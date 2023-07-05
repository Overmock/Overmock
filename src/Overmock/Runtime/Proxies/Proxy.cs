using System.Reflection;

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
        protected ProxyOverrideContext? ___context;
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
        public void InitializeOvermockContext(ProxyOverrideContext context)
        {
            ___context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public IOvermock MethodCall(string name, object p1, IList<IOvermock> p3, int p2)
        {
            return (IOvermock)HandleMethodCall((MethodInfo)MethodBase.GetCurrentMethod()!, name, p1, p2, p3)!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object HandleMethodCall(MethodInfo method, params object[] parameters)
        {
            var handle = ___context.Get(method);
            var result = handle.Handle(parameters);
            return result.Result;
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