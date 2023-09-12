using Kimono.Core.Internal;
using System;
using System.Reflection;

namespace Kimono.Core
{
    public class Interceptor<T> : IInterceptor<T>, IProxyContainer where T : class
    {   
        private ProxyContext? _proxyContext;
        private readonly T? _target;

        public bool BuildInvoker => !(_target is null);

        public Interceptor(T? target = null)
        {
            _target = target;
        }

        //public static implicit operator T(Interceptor<T> interceptor)
        //{
        //    return interceptor._proxy!;
        //}

        public object? HandleInvocation(int methodId, Type[] genericParameters, object[] parameters)
        {
            ref var metadata = ref _proxyContext.GetMethod(methodId);

            var invocation = new Invocation(metadata.GetInvoker()!)
            {
                GenericParameters = genericParameters,
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
            invocation.Invoke();
        }

        private static class Types
        {
            public static ParameterInfo[] EmptyParameters = new ParameterInfo[0];
        }
    }
}