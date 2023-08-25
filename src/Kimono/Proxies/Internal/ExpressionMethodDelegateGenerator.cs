using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kimono.Internal
{
    internal sealed class ExpressionMethodDelegateGenerator : DelegateFactory
    {
        protected override TDelegate CreateMethodInvoker<TDelegate>(MethodInfo method, Type delegateType, IReadOnlyList<RuntimeParameter> parameters, bool returnsVoid = false)
        {
            if (returnsVoid)
            {
                return GenerateAction<TDelegate>(method, parameters);
            }

            return GenerateFunc<TDelegate>(method, parameters);
        }

        private static TDelegate GenerateAction<TDelegate>(MethodInfo method, IReadOnlyList<RuntimeParameter> parameters)
            where TDelegate : Delegate
        {
            var (methodCallExpression, parameterExpressions) = GenerateMethodCall(method, parameters);
            return GenerateFinalDelegate<TDelegate>(methodCallExpression, parameterExpressions);
        }

        private static TDelegate GenerateFunc<TDelegate>(MethodInfo method, IReadOnlyList<RuntimeParameter> parameters)
            where TDelegate : Delegate
        {
            var (methodCallExpression, parameterExpressions) = GenerateMethodCall(method, parameters);

            return GenerateFinalDelegate<TDelegate>(
                methodCallExpression,
                parameterExpressions,
                methodCall => Expression.Convert(methodCall, Constants.ObjectType)
            );
        }

        private static (MethodCallExpression, ParameterExpression[]) GenerateMethodCall(MethodInfo method, IReadOnlyList<RuntimeParameter> parameters)
        {
#if NET7_0
            var span = CollectionsMarshal.AsSpan(parameters.ToList());
#else
            var span = parameters.ToArray().AsSpan();
#endif
            var parameterExpressions = new ParameterExpression[span.Length + 1];
            var convertedParameters = new UnaryExpression[span.Length];
            ref var reference = ref MemoryMarshal.GetReference(span);

            for (int i = 0; i < span.Length; i++)
            {
                ref var parameter = ref Unsafe.Add(ref reference, i);
                var parameterExpression = Expression.Parameter(
                    Constants.ObjectType, parameter.Name
                );
                parameterExpressions[i + 1] = parameterExpression;
                convertedParameters[i] = Expression.Convert(
                    parameterExpression,
                    parameter.Type
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
