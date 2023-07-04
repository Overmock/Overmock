using System.Reflection;

namespace Overmock.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public class OvermockContext
	{
		private readonly IDictionary<Guid, OverrideContext> _overrides = new Dictionary<Guid, OverrideContext>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="methodId"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public OvermockContext Add(Guid methodId, OverrideContext context)
		{
			_overrides.Add(methodId, context);

			return this;
		}

		/// <summary>
		/// Gets the <see cref="OverrideContext" /> for the given <see cref="MethodInfo" />.
		/// </summary>
		/// <param name="method">The key used to get the <see cref="OverrideContext" />.</param>
		/// <returns>The <see cref="OverrideContext" />.</returns>
		public IOverrideHandler Get(MethodInfo method)
		{
			var attributeValue = method.GetCustomAttribute<OvermockAttribute>()?.Value as string;

			if (!Guid.TryParse(attributeValue, out Guid methodId) || !_overrides.ContainsKey(methodId))
			{
				return new UnregisteredOverrideHandler(method);
			}

			return new OverrideHandler(_overrides[methodId]);
		}
	}
}
