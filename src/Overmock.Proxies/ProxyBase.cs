using System.Reflection;

namespace Overmock.Proxies
{
    /// <inheritdoc />
    public abstract class ProxyBase<T> : IProxy<T> where T : class
    {
#pragma warning disable CA1051 // Do not declare visible instance fields
        protected ProxyContext? ___context;
#pragma warning restore CA1051 // Do not declare visible instance fields

		private Func<RuntimeContext, object[], object>? _callback;

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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		void IProxy.InitializeProxyContext(ProxyContext context)
        {
            ___context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected object? HandleMethodCall(MethodInfo method, params object[] parameters)
        {
            var handle = ___context.Get(method);
            var result = handle.Handle(parameters);
            return result.Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		Type IProxy.GetTargetType()
		{
            return TargetType;
		}

		internal void RegisterCallback(Func<RuntimeContext, object[], object> memberInvoked)
		{
			_callback = memberInvoked;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		protected object? InvokeTargetMember(RuntimeContext context, object[] parameters)
		{
            return _callback?.Invoke(context, parameters);
		}
	}
}