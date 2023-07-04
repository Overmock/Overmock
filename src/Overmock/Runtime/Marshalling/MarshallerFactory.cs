namespace Overmock.Runtime.Marshalling
{
    /// <summary>
    /// 
    /// </summary>
    public class MarshallerFactory : IMarshallerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="argsProvider"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IMarshaller Create(IOvermock target, Action<SetupArgs>? argsProvider = null)
        {
            if (target.IsInterface())
            {
                return new InterfaceProxyMarshaller(target, argsProvider);
            }

            if (target.IsDelegate())
            {
                //return CreateInterfaceProxy(target);
            }

            throw new NotImplementedException();
        }
    }
}