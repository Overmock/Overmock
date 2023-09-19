using Kimono;

namespace Kimono.Tests.Proxies
{
    public interface ITGenTT1T2T3NoArgs
    {
        T TGArgsTArgsInt<T, T1, T2, T3>(int id);
    }

    public class ITGenTT1T2T3NoArgsClass : ProxyBase, ITGenTT1T2T3NoArgs
    {
        public ITGenTT1T2T3NoArgsClass(IInterceptor<ITGenTT1T2T3NoArgs> interceptor) : base(interceptor)
        {
        }

        public T TGArgsTArgsInt<T, T1, T2, T3>(int id)
        {
            Type t = typeof(T);
            Type t1 = typeof(T1);
            Type t2 = typeof(T2);
            Type t3 = typeof(T3);
            const int methodId = 90001;
            return (T)HandleMethodCall(methodId, new[] { t, t1, t2, t3 }, new object[] { id });
        }
    }
}
