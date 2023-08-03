namespace Overmock.Proxies
{
    public interface IProxyCache
    {
        bool Contains(Type type);
        
        object? Get(Type type);
        
        object Set<T>(Type type, T value);
        
        bool TryGet(Type type, out object? value);

        bool TrySet(Type type, object value);
    }
}