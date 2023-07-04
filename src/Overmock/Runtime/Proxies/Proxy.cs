using System.Reflection;
using System.Xml.Linq;

namespace Overmock.Runtime.Proxies
{
    /// <inheritdoc />
    public abstract class Proxy<T> : IProxy<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
#pragma warning disable CA1051 // Do not declare visible instance fields
        // ReSharper disable once InconsistentNaming
        protected OvermockContext? ___context;
#pragma warning restore CA1051 // Do not declare visible instance fields

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        protected Proxy(IOvermock target)
        {
            Target = target;
        }

        /// <inheritdoc />
        public IOvermock Target { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void InitializeOvermockContext(OvermockContext context)
        {
            ___context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected object? HandleMethodCall(MethodInfo method, object[] parameters)
        {
            var handle = ___context?.Get((MethodInfo)MethodBase.GetCurrentMethod()!);
            var result = handle?.Handle(parameters);
            return result?.Result;
        }
    }

    ///// <inheritdoc />
    //public class InterfaceProxy<TInterface> : Proxy<TInterface> where TInterface : class
    //{
    //    /// <inheritdoc />
    //    public InterfaceProxy(IOvermock target) : base(target)
    //    {
    //    }
    //}

    ///// <inheritdoc />
    //public class DelegateProxy<T> : Proxy<T> where T : class
    //{
    //    public DelegateProxy(IOvermock target) : base(target)
    //    {
    //    }
    //}
}