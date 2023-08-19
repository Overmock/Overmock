
namespace Kimono.Tests.Proxies
{
    public interface IStringArgsIntDoubleString
    {
        string StringArgsIntDoubleString(int id, double balance, string status);
    }

    public class StringArgsIntDoubleStringClass : IStringArgsIntDoubleString
    {
        public string StringArgsIntDoubleString(int id, double balance, string status)
        {
            return $"Account Id: {id} with the balance of {balance:$} with status: {status}";
        }
    }
}
