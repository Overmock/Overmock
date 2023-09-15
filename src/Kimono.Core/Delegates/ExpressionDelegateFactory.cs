using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kimono.Delegates
{
    /// <summary>
    /// Uses Expressions to compile delegates used to invoke members on proxies.
    /// </summary>
    public sealed class ExpressionDelegateFactory : DelegateFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <param name="method"></param>
        /// <param name="invocation"></param>
        /// <param name="delegateType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected override TDelegate CreateActionInvoker<TDelegate>(MethodMetadata metadata, Type delegateType, IInvocation invocation)
        {
            var (methodCallExpression, parameterExpressions) = GenerateMethodCall(metadata, invocation, metadata.Parameters);
            return GenerateFinalDelegate<TDelegate>(methodCallExpression, parameterExpressions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <param name="method"></param>
        /// <param name="context"></param>
        /// <param name="delegateType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected override TDelegate CreateFuncInvoker<TDelegate>(MethodMetadata metadata, Type delegateType, IInvocation invocation)
        {
            var (methodCallExpression, parameterExpressions) = GenerateMethodCall(metadata, invocation, metadata.Parameters);

            return GenerateFinalDelegate<TDelegate>(
                methodCallExpression,
                parameterExpressions,
                methodCall => Expression.Convert(methodCall, Types.Object)
            );
        }

        private static (MethodCallExpression, ParameterExpression[]) GenerateMethodCall(MethodMetadata metadata, IInvocation invocation, ParameterInfo[] parameters)
        {
            var method = PrepareGenericMethod(metadata, invocation.GenericParameters);
            var span = parameters.AsSpan();
            var parameterExpressions = new ParameterExpression[span.Length + 1];
            var convertedParameters = new UnaryExpression[span.Length];
            ref var reference = ref MemoryMarshal.GetReference(span);

            for (int i = 0; i < span.Length; i++)
            {
                ref var parameter = ref Unsafe.Add(ref reference, i);
                var parameterExpression = Expression.Parameter(
                    Types.Object, parameter.Name
                );
                parameterExpressions[i + 1] = parameterExpression;
                convertedParameters[i] = Expression.Convert(
                    parameterExpression,
                    parameter.ParameterType
                );
            }

            var target = Expression.Parameter(typeof(object), "target");
            parameterExpressions[0] = target;

            var methodCallExpression = Expression.Call(
                Expression.Convert(target, method.DeclaringType!),
                method.Name,
                method.IsGenericMethod ? method.GetGenericArguments() : Type.EmptyTypes,
                convertedParameters
            );

            return (methodCallExpression, parameterExpressions);
        }

        private static TDelegate GenerateFinalDelegate<TDelegate>(MethodCallExpression methodCallExpression, ParameterExpression[] parameterExpressions, Func<MethodCallExpression, Expression>? converter = null)
            where TDelegate : Delegate
        {
            var lambda = Expression.Lambda<TDelegate>(
                converter == null ? methodCallExpression : converter(methodCallExpression),
                parameterExpressions);

            var compiledExpression = lambda.Compile();
            return compiledExpression;
        }
    }
}
