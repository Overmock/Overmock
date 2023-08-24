namespace Kimono
{
    /// <summary>
    /// Interface IInvocationHandlerProvider
    /// </summary>
    public interface IInvocationHandlerProvider
    {
        /// <summary>
        /// Gets the handler.
        /// </summary>
        /// <returns>IInvocationHandler.</returns>
        IInvocationHandler GetHandler();
    }
}
