namespace Kimono
{
    public interface IProxyCache
    {
        bool Contains(Type type);

		IProxyGenerator? Get(Type type);

		IProxyGenerator Set<T>(Type type, T value) where T : IProxyGenerator;
        
        bool TryGet(Type type, out IProxyGenerator? value);

        bool TrySet(Type type, IProxyGenerator value);
    }
}