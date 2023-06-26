using System.Reflection;

namespace Overmock.Runtime
{
	/// <summary>
	/// 
	/// </summary>
	public class OvermockContext
	{
		private readonly IDictionary<MethodInfo, OverrideContext> _overrides = new Dictionary<MethodInfo, OverrideContext>();

		internal OvermockContext Add(MethodInfo method, OverrideContext context)
		{
			_overrides.Add(method, context);

			return this;
		}

		/// <summary>
		/// Gets the <see cref="OverrideContext" /> for the given <see cref="MethodInfo" />.
		/// </summary>
		/// <param name="method">The key used to get the <see cref="OverrideContext" />.</param>
		/// <returns>The <see cref="OverrideContext" />.</returns>
		public IOverrideHandler Get(MethodInfo method)
		{
			if (!_overrides.ContainsKey(method))
			{
				return new UnregisteredOverrideHandler(method);
			}

			return new OverrideHandler(_overrides[method]);
		}

	}
}
