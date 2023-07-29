using Overmock.Runtime.Proxies;
using System.Reflection;

namespace Overmock.Tests.Mocks.Properties
{
    public interface IPropertiesWithGet
    {
        public int Int { get; }

        public Model Model { get; }

        public string String { get; }

        public List<Model> ListOfModels { get; }
    }

    public class IPropertiesWithGetImpl : Proxy<IPropertiesWithGet>, IPropertiesWithGet
    {
        public IPropertiesWithGetImpl(IOvermock target) : base(target)
        {
        }

        public int Int => (int)HandleMethodCall((MethodInfo)MethodBase.GetCurrentMethod());

        public Model Model => throw new NotImplementedException();

        public string String => throw new NotImplementedException();

        public List<Model> ListOfModels => throw new NotImplementedException();
    }
}
