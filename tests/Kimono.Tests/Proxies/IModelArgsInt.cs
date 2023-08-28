
namespace Kimono.Tests.Proxies
{
    public interface IModelArgsInt
    {
        Model ModelArgsInt(int value);
    }

    public class ModelArgsIntClass : IModelArgsInt
    {
        public Model ModelArgsInt(int value)
        {
            return new Model();
        }
    }

    public class ICallModelArgsInt
    {
        public object CallModelArgsInt(object obj, object value)
        {
            return ((IModelArgsInt)obj).ModelArgsInt((int)value);
        }
    }
}
