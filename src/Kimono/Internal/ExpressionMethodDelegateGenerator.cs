using Kimono.Internal.MethodInvokers;
using Kimono.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Kimono.Internal
{
    internal class ExpressionMethodDelegateGenerator : IMethodDelegateGenerator
    {
        public IMethodDelegateInvoker Generate(RuntimeContext context, MethodInfo method)
        {
            var parameters = context.GetParameters();

            if (method.ReturnType == Constants.VoidType)
            {
                return GenerateAction(context, method, parameters);
            }

            return GenerateFunc(context, method, parameters);

        }

        private static IMethodDelegateInvoker GenerateAction(RuntimeContext context, MethodInfo method, IReadOnlyList<RuntimeParameter> parameters)
        {
            if (parameters.Count == 0)
            {
                return new ActionObjectMethodInvoker(() => CompileExpression<Action<object?>>(context, method, parameters));
            }

            return new MethodInfoDelegateInvoker(method);
        }

        private static IMethodDelegateInvoker GenerateFunc(RuntimeContext context, MethodInfo method, IReadOnlyList<RuntimeParameter> parameters)
        {
            throw new NotImplementedException();
        }

        private static TDelegate CompileExpression<TDelegate>(RuntimeContext context, MethodInfo method, IReadOnlyList<RuntimeParameter> parameters)
        {
            var span = CollectionsMarshal.AsSpan(parameters.ToList());
            var parameterExpressions = new ParameterExpression[span.Length];
            var convertedParameters = new UnaryExpression[span.Length];
            ref var reference = ref MemoryMarshal.GetReference(span);

            for (int i = 0; i < span.Length; i++)
            {
                ref var parameter = ref Unsafe.Add(ref reference, i);
                var parameterExpression = Expression.Parameter(
                    Constants.ObjectType, parameter.Name
                );
                parameterExpressions[i] = parameterExpression;
                convertedParameters[i] = Expression.Convert(
                    parameterExpression,
                    parameter.Type
                );
            }

            var target = Expression.Parameter(typeof(object), "target");

            Expression methodCall = Expression.Call(
                Expression.Convert(target, method.DeclaringType!),
                Constants.KimonoDeleateTypeNameFormat.ApplyFormat(method.Name),
                method.IsGenericMethod ? method.GetGenericArguments() : Type.EmptyTypes,
                parameterExpressions
            );

            var lambda = Expression.Lambda<TDelegate>(methodCall, parameterExpressions);
            var compiledExpression = lambda.Compile();
            return compiledExpression;
        }
    }
}
