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

    public class Interceptor<T> : IInterceptor<T>, IProxyContainer
    {   
        private ProxyContext? _proxyContext;


        public object? HandleInvocation(int methodId, IProxy<T> proxyBase, Type[] genericParameters, object[] parameters)
        {
            var metadata = _proxyContext.GetMethod(methodId);

            var invocation = new Invocation
            {
                GenericParameters = Type.EmptyTypes,//metadata.GenericParameters,
                Method = metadata.TargetMethod,
                Parameters = parameters,
                //ParameterTypes = metadata.ParameterTypes
            };

            HandleInvocation(invocation);

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