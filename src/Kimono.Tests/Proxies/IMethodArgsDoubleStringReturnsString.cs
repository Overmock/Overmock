
namespace Kimono.Tests.Proxies
{
    public interface IMethodArgsDoubleStringReturnsString
    {
        string MethodArgsDoubleStringReturnsString(double balance, string status);
    }

    public class MethodArgsDoubleStringReturnsStringClass : IMethodArgsDoubleStringReturnsString
    {
        public string MethodArgsDoubleStringReturnsString(double balance, string status)
        {
            return string.Join(", ", balance, status);
        }
    }
}
