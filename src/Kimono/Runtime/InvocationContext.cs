using Kimono.Proxies;
using Kimono.Runtime;
using System.Reflection;

namespace Kimono
{
    /// <summary>
    /// Class InvocationContext.
    /// </summary>
    public class InvocationContext : IInvocationContext
	{
        private readonly IInterceptor _interceptor;
        private readonly RuntimeContext _runtimeContext;
        private readonly Func<Parameters> _parametersProvider;

        private readonly Func<object[]> _parameterValuesProvider;
        private bool _targetInvoked;
        private Parameters? _parameters;
        private object? _defaultReturnValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationContext" /> class.
        /// </summary>
        /// <param name="runtimeContext">The runtime context.</param>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="parameterValues">The parameter values.</param>
        public InvocationContext(RuntimeContext runtimeContext, IInterceptor interceptor, RuntimeParameter[] parameters, object[] parameterValues)
        {
            _runtimeContext = runtimeContext;
            _interceptor = interceptor;
            _parameterValuesProvider = () => parameterValues;
            _parametersProvider = () => new Parameters(parameters, parameterValues);
		}

		/// <summary>
		/// Gets the interceptor.
		/// </summary>
		/// <value>The interceptor.</value>
		public IInterceptor Interceptor => _interceptor;

		/// <summary>
		/// Gets the name of the member.
		/// </summary>
		/// <value>The name of the member.</value>
		public string MemberName => _runtimeContext.MemberName;

        /// <summary>
        /// Gets the member.
        /// </summary>
        /// <value>The member.</value>
        public MemberInfo Member => _runtimeContext.ProxiedMember.Member;

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <value>The method.</value>
        public MethodInfo Method => _runtimeContext.ProxiedMember.Method;

		/// <summary>
		/// Gets the parameters.
		/// </summary>
		/// <value>The parameters.</value>
		public Parameters Parameters
        {
            get
            {
                if (_parameters is null)
                {
                    _parameters = _parametersProvider();
                }

                return _parameters;
            }
        }

		/// <summary>
		/// Gets or sets the return value.
		/// </summary>
		/// <value>The return value.</value>
		public object? ReturnValue { get; set; }

        /// <inheritdoc />
        public bool TargetInvoked => _targetInvoked;

		/// <summary>
		/// Gets the specified name.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name">The name.</param>
		/// <returns>T.</returns>
		public T GetParameter<T>(string name)
		{
			return Parameters.Get<T>(name);
		}

        /// <summary>
        /// Invokes the target.
        /// </summary>
        /// <param name="setReturnValue">if set to <c>true</c> [set return value].</param>
        /// <param name="force">if set to <c>true</c> forces the call to be invoked regardless if it's already been called successfully.</param>
        public void Invoke(bool setReturnValue = true, bool force = false)
		{
			// If the target's member has already been called and they
			// haven't forced the invocation, then return to the caller;
			if (_targetInvoked && !force) { return; }

            var invoker = _runtimeContext.GetMethodInvoker();

            var target = _interceptor.GetTarget();

            // If the target's null then we don't have one to call;
            if (target is null) { return; }

			var returnValue = invoker.Invoke(target, _parameterValuesProvider());

            _targetInvoked = true;

            if (setReturnValue)
			{
				ReturnValue = returnValue;
			}
		}

		/// <summary>
		/// Gets the return type default value.
		/// </summary>
		/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
		internal object? GetReturnTypeDefaultValue()
		{
			return _defaultReturnValue ??= DefaultReturnValueCache.GetDefaultValue(Method.ReturnType);
		}

		/// <summary>
		/// Members the type of the returns value.
		/// </summary>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		internal bool ReturnsValueType()
		{
            var member = Member;

			if (member is MethodInfo method)
			{
				return method.ReturnType.IsValueType;
			}

			if (member is PropertyInfo property)
			{
				return property.PropertyType.IsValueType;
			}

			return false;
        }

        //internal InvocationContext Reset(object[] parameters)
        //{
        //    _arguments = parameters;
        //    _parameters?.SetParameterValues(parameters);
            
        //    TargetInvoked = false;

        //    return this;
        //}
    }
}