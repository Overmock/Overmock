
namespace Kimono.Tests.Proxies
{
    public interface ITGArgsTArgsInt
    {
        T TGArgsTArgsInt<T>(int id);
    }

    public class ITGArgsTArgsIntClass : ITGArgsTArgsInt
    {
        public T TGArgsTArgsInt<T>(int id)
        {
            return default;
        }
    }
}
