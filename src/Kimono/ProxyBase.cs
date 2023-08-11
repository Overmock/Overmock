using System.Reflection;

namespace Kimono
{
    /// <inheritdoc />
    public abstract class ProxyBase<T> : IProxy<T> where T : class
    {
#pragma warning disable CA1051 // Do not declare visible instance fields
		/// <summary>
		/// The context
		/// </summary>
		protected ProxyContext? ___context;
#pragma warning restore CA1051 // Do not declare visible instance fields

		/// <summary>
		/// Initializes a new instance of the <see cref="ProxyBase{T}"/> class.
		/// </summary>
		protected ProxyBase()
        {
            TargetType = typeof(T);
        }

		/// <summary>
		/// The <see cref="Target" />s <see cref="Type" />.
		/// </summary>
		/// <value>The type of the target.</value>
		public Type TargetType { get; }

		/// <summary>
		/// Gets the interceptor.
		/// </summary>
		/// <value>The interceptor.</value>
		public IInterceptor Interceptor { get; private set; }

		/// <summary>
		/// Initializes the proxy context.
		/// </summary>
		/// <param name="interceptor">The interceptor.</param>
		/// <param name="context">The context.</param>
		void IProxy.InitializeProxyContext(IInterceptor interceptor, ProxyContext context)
        {
            Interceptor = interceptor;
            ___context = context;
        }

		/// <summary>
		/// Handles the method call.
		/// </summary>
		/// <param name="methodId">The method identifier.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
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
		/// Gets the type of the target.
		/// </summary>
		/// <returns>Type.</returns>
		Type IProxy.GetTargetType()
        {
            return TargetType;
        }
    }
}