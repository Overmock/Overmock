using System.ComponentModel;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IProxyContextSetter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        void SetProxyContext(ProxyContext context);
    }
}