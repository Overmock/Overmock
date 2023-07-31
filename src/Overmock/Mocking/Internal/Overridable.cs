using System.Reflection;

namespace Overmock.Mocking.Internal
{
	internal abstract class Overridable : IOverridable
	{
		public IEnumerable<IOverride> GetOverrides()
		{
			var overrides = new List<IOverride>();

			AddOverridesTo(overrides);

			return overrides.AsReadOnly();
		}

		public abstract MemberInfo GetTarget();

		protected abstract void AddOverridesTo(List<IOverride> overrides);
	}
}