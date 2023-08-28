
namespace Kimono.Tests.Proxies
{
    public interface ITGenTArgsTInt
    {
        T TGenTArgsTInt<T>(T t, int id);
    }

    public class ITGenTArgsTIntClass : ITGenTArgsTInt
    {
        public T TGenTArgsTInt<T>(T t, int id)
        {
            return default;
        }
    }
}
