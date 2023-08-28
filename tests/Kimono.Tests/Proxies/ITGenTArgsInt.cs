
namespace Kimono.Tests.Proxies
{
    public interface ITGenTArgsInt
    {
        T TGenTArgsInt<T>(int id);
    }

    public class ITGenTArgsIntClass : ITGenTArgsInt
    {
        public T TGenTArgsInt<T>(int id)
        {
            return default;
        }
    }
}
