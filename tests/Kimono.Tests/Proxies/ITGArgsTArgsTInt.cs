
namespace Kimono.Tests.Proxies
{
    public interface ITGArgsTArgsTInt
    {
        T TGArgsTArgsTInt<T>(T t, int id);
    }

    public class ITGArgsTArgsTIntClass : ITGArgsTArgsTInt
    {
        public T TGArgsTArgsTInt<T>(T t, int id)
        {
            return default;
        }
    }
}
