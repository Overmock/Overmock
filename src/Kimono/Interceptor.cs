using Kimono.Internal;
using System;
using System.Reflection;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Interceptor<T> : IInterceptor<T>, IProxyContextSetter where T : class
    {   
        private ProxyContext? _proxyContext;
        private readonly T? _target;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public Interceptor(T? target = null)
        {
            _target = target;
        }

        /// <summary>
        /// 
        /// </summary>
        bool IInterceptor<T>.BuildInvoker => !(_target is null);

        /// <summary>
        /// 
        /// </summary>
        protected T? Target => _target;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="genericParameters"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        object? IInterceptor.HandleInvocation(int methodId, Type[] genericParameters, object[] parameters)
        {
            ref var metadata = ref _proxyContext.GetMethod(methodId);

            var invocation = new Invocation(metadata.GetDelegateInvoker()!)
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

        /// <inheritdoc />
        void IProxyContextSetter.SetProxyContext(ProxyContext context)
        {
            _proxyContext ??= context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        protected virtual void HandleInvocation(IInvocation invocation)
        {
            invocation.Invoke();
        }

        private static class Types
        {
            public static ParameterInfo[] EmptyParameters = Array.Empty<ParameterInfo>();
        }
    }
}