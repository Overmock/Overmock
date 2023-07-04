using System.Reflection;

namespace Overmock.Runtime.Marshalling
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ProxyMarshaller : IMarshaller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="argsProvider"></param>
        protected ProxyMarshaller(IOvermock target, Action<SetupArgs>? argsProvider)
        {
            Target = target;
            ArgsProvider = argsProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        public IOvermock Target { get; }

        /// <summary>
        /// 
        /// </summary>
        public Action<SetupArgs>? ArgsProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract T? Marshal<T>() where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected static string GetAssemblyName(IOvermock target)
        {
            return string.Format(RuntimeConstants.AssemblyNameFormat, target.TypeName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected static AssemblyName GetAssemblyDllName(IOvermock target)
        {
            return new AssemblyName(
                string.Format(RuntimeConstants.AssemblyDllNameFormat, GetAssemblyName(target))
            );
        }
    }
}