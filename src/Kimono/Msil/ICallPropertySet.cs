using System;

namespace Kimono.Msil
{
    /// <summary>
    /// Interface ICallPropertySetter
    /// </summary>
    public interface ICallPropertySet
    {
        /// <summary>
        /// Calls the set.
        /// </summary>
        /// <returns>IEmitter.</returns>
        IEmitter CallSet();
    }

    /// <summary>
    /// Interface ICallPropertySetter
    /// </summary>
    /// <typeparam name="TDelegate">The type of the t delegate.</typeparam>
    public interface ICallPropertySet<TDelegate> where TDelegate : Delegate
    {
        /// <summary>
        /// Calls the set.
        /// </summary>
        /// <returns>IEmitter&lt;TDelegate&gt;.</returns>
        IEmitter<TDelegate> CallSet();
    }
}
