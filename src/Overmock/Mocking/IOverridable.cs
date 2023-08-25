using System.Collections.Generic;
using System.Reflection;

namespace Overmock.Mocking
{
    /// <summary>
    /// Interface IOverridable
    /// </summary>
    public interface IOverridable : IVerifiable
    {
        /// <summary>
        /// Gets the overrides for this overmock.
        /// </summary>
        /// <returns>IEnumerable&lt;IOverride&gt;.</returns>
        IEnumerable<IOverride> GetOverrides();

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <returns>MemberInfo.</returns>
        MemberInfo GetTarget();
    }
}