using Kimono.Runtime;
using System;
using System.Reflection;

namespace Kimono.Core
{
    public interface IInterceptor : IFluentInterface
    {
    }

    public interface IInterceptor<T> : IInterceptor
    {
        object? HandleInvocation(int methodId, IProxy<T> proxyBase, Type[] genericParameters, object[] parameters);
    }

    public class Interceptor<T> : IInterceptor<T>, IProxyContainer where T : class
    {   
        private ProxyContext? _proxyContext;
        private readonly T? _target;

        public Interceptor(T? target = null)
        {
            _target = target;
        }

        //public static implicit operator T(Interceptor<T> interceptor)
        //{
        //    return interceptor._proxy!;
        //}

        public object? HandleInvocation(int methodId, IProxy<T> proxyBase, Type[] genericParameters, object[] parameters)
        {
            var metadata = _proxyContext.GetMethod(methodId);

            var invocation = new Invocation(metadata.GetInvoker()!)
            {
                GenericParameters = metadata.GenericParameters,
                Method = metadata.TargetMethod,
                Parameters = parameters,
                ReturnType = metadata.ReturnType,
                ParameterTypes = metadata.Parameters,
                IsProperty = metadata.IsProperty,
                Target = _target
            };

            HandleInvocation(invocation);

            if (invocation.ReturnValue is null && invocation.ReturnType.IsValueType)
            {
                return DefaultReturnValueCache.GetDefaultValue(invocation.ReturnType);
            }

            return invocation.ReturnValue;
        }

        void IProxyContainer.SetProxyContext(ProxyContext context)
        {
            _proxyContext ??= context;
        }

        protected virtual void HandleInvocation(IInvocation invocation)
        {
        }

        private static class Types
        {
            public static ParameterInfo[] EmptyParameters = new ParameterInfo[0];
        }
    }
}