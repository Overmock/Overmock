using System.Reflection;

namespace Overmock.Mocking.Internal
{
	/// <summary>
	/// Class Overridable.
	/// Implements the <see cref="IOverridable" />
	/// </summary>
	/// <seealso cref="IOverridable" />
	internal abstract class Overridable : Verifiable, IOverridable
	{
        private readonly IOverride _exceptionOverride;

        protected Overridable(string name)
        {
            _exceptionOverride = new ThrowExceptionOverride(new UnhandledMemberException(name));
        }

        /// <inheritdoc />
        public IEnumerable<IOverride> GetOverrides()
		{
			var overrides = new List<IOverride>();

			AddOverridesTo(overrides);

			if (overrides.Count == 0)
			{
				overrides.Add(_exceptionOverride);
			}

			return overrides.AsReadOnly();
		}

		/// <summary>
		/// Gets the target.
		/// </summary>
		/// <returns>MemberInfo.</returns>
		public abstract MemberInfo GetTarget();

        protected override void Verify()
        {
            var overrides = GetOverrides();

            foreach (var item in overrides)
            {
                item.Verify();
            }
        }

        /// <summary>
        /// Adds the overrides to.
        /// </summary>
        /// <param name="overrides">The overrides.</param>
        protected abstract void AddOverridesTo(List<IOverride> overrides);
	}
}