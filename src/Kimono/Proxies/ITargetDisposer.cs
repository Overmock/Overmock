namespace Kimono.Proxies
{
    /// <summary>
    /// Interface ITargetDisposer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITargetDisposer<T> : IDisposable where T : class, IDisposable
    {
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        void Dispose(bool disposing);
    }
}
