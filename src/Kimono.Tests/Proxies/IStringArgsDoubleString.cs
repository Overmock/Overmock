
namespace Kimono.Tests.Proxies
{
    public interface IStringArgsDoubleString
    {
        string StringArgsDoubleString(double balance, string status);
    }

    public class StringArgsDoubleStringClass : IStringArgsDoubleString
    {
        public string StringArgsDoubleString(double balance, string status)
        {
            return string.Join(", ", balance, status);
        }
    }
}
