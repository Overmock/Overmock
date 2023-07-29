using System.Reflection;

namespace Overmock.Runtime
{
	/// <summary>
	/// The context for an overridden member.
	/// </summary>
	public class RuntimeContext
	{
		private readonly MemberInfo _target;
		private readonly object? _defaultReturnValue;

		// TODO: This needs to handle more that one override.
		private readonly List<RuntimeParameter> _parameters = new List<RuntimeParameter>();

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeContext"/> class.
		/// </summary>
		/// <param name="overmock"></param>
		/// <param name="target"></param>
		/// <param name="parameters">The parameters.</param>
		public RuntimeContext(ICallable overmock, MethodInfo? target, IEnumerable<RuntimeParameter> parameters)
		{
			_target = target ?? overmock.GetTarget();
			_parameters.AddRange(parameters);
			_defaultReturnValue = overmock.GetDefaultReturnValue();

			Overrides = overmock.GetOverrides();
		}

		/// <summary>
		/// Gets the name of the Override.
		/// </summary>
		public string MemberName => _target.Name;

		/// <summary>
		/// Gets the number of parameters for this overmock.
		/// </summary>
		public int ParameterCount => _parameters.Count;

		/// <summary>
		/// The overrides for the current Overmock.
		/// </summary>
		public IEnumerable<IOverride> Overrides { get; }

        /// <summary>
		/// Gets the specified name.
		/// </summary>
		/// <typeparam name="TParameter">The type of the parameter.</typeparam>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public TParameter? Get<TParameter>(string name)
		{
			var param = _parameters.Find(p => p.Name == name);

			if (param == null || param.Type != typeof(TParameter))
			{
				return default;
			}

			return (TParameter)param.Value;
		}

		/// <summary>
		/// Gets the specified index.
		/// </summary>
		/// <typeparam name="TParameter">The type of the parameter.</typeparam>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public TParameter? Get<TParameter>(int index)
		{
			var param = _parameters.ElementAtOrDefault(index);

			if (param == null || param.Type != typeof(TParameter))
			{
				return default;
			}

			return (TParameter)param.Value;
		}

		internal object? GetDefaultValueTypeValue()
		{
			return _defaultReturnValue;
		}

		internal bool IsValueType()
		{
			if (_target is MethodInfo method)
			{
				return method.ReturnType.IsValueType;
			}

			if (_target is PropertyInfo property)
			{
				return property.PropertyType.IsValueType;
			}

			return false;
		}

		internal void SetParameterValue(int i, object value)
		{
			var parameter = _parameters.ElementAtOrDefault(i);

			if (parameter != null)
			{
				parameter.Value = value;
			}
		}
	}
}