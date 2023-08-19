namespace Kimono.Emit
{
    /// <summary>
    /// Interface ICallPropertyGetter
    /// </summary>
    public interface ICallPropertyGet
    {
        /// <summary>
        /// Calls the get.
        /// </summary>
        /// <returns>IEmitter.</returns>
        IEmitter CallGet();
    }

    /// <summary>
    /// Interface ICallPropertyGetter
    /// </summary>
    /// <typeparam name="TDelegate">The type of the t delegate.</typeparam>
    public interface ICallPropertyGet<TDelegate> where TDelegate : Delegate
    {
        /// <summary>
        /// Calls the get.
        /// </summary>
        /// <returns>IEmitter&lt;TDelegate&gt;.</returns>
        IEmitter<TDelegate> CallGet();
    }
}
