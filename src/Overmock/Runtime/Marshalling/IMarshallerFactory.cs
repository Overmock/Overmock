namespace Overmock.Runtime.Marshalling
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMarshallerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="argsProvider"></param>
        /// <returns></returns>
        IMarshaller Create(IOvermock target, Action<SetupArgs>? argsProvider = null);
    }
}