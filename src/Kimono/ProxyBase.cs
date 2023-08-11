using System.Reflection;

namespace Kimono
{
    /// <inheritdoc />
    public abstract class ProxyBase<T> : IProxy<T> where T : class
    {
#pragma warning disable CA1051 // Do not declare visible instance fields
        protected ProxyContext? ___context;
#pragma warning restore CA1051 // Do not declare visible instance fields

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        protected ProxyBase()
        {
            TargetType = typeof(T);
        }

        /// <summary>
        /// The <see cref="Target" />s <see cref="Type" />.
        /// </summary>
        public Type TargetType { get; }

        public IInterceptor Interceptor { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        void IProxy.InitializeProxyContext(IInterceptor interceptor, ProxyContext context)
        {
            Interceptor = interceptor;
            ___context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected object? HandleMethodCall(int methodId, params object[] parameters)
        {
			var invocationContext = ___context.GetInvocationContext(methodId, this, parameters);

			Interceptor.MemberInvoked(invocationContext);

			if (invocationContext.ReturnValue == null && invocationContext.MemberReturnsValueType())
			{
                return invocationContext.GetReturnTypeDefaultValue();
			}

            return invocationContext.ReturnValue;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Type IProxy.GetTargetType()
        {
            return TargetType;
        }
    }
}