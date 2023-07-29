using System.Reflection;
using System.Reflection.Emit;

namespace Overmock.Runtime.Proxies
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
        public Action<SetupArgs>? ArgsProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        protected IOvermock Target { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Marshal<T>() where T : class
        {
            return (T)Marshal();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public object Marshal()
        {
            var context = CreateContext();
            return MarshalCore(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract IMarshallerContext CreateContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marshallerContext"></param>
        /// <returns></returns>
        protected abstract object MarshalCore(IMarshallerContext marshallerContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected static string GetName(IOvermock target) => Constants.AssemblyAndTypeNameFormat.ApplyFormat(target.TypeName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected static AssemblyName GetAssemblyName(IOvermock target) => new(Constants.AssemblyDllNameFormat.ApplyFormat(GetName(target)));

        /// <summary>
		/// 
		/// </summary>
		protected interface IMarshallerContext
        {
            /// <summary>
            /// 
            /// </summary>
			IOvermock Target { get; }

            /// <summary>
            /// 
            /// </summary>
			TypeBuilder TypeBuilder { get; }

            /// <summary>
            /// 
            /// </summary>
            ProxyOverrideContext OvermockContext { get; }
        }
    }
}