
namespace Kimono.Tests.Proxies
{
    public interface IMethodArgsIntDoubleStringReturnsString
    {
        string MethodArgsIntDoubleStringReturnsString(int id, double balance, string status);
    }

    public class MethodArgsIntDoubleStringReturnsStringClass : IMethodArgsIntDoubleStringReturnsString
    {
        public string MethodArgsIntDoubleStringReturnsString(int id, double balance, string status)
        {
            return $"Account Id: {id} with the balance of {balance:$} with status: {status}";
        }
    }
}
