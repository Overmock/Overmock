using Kimono.Proxies;

namespace Overmock.Tests.Mocks.Properties
{
    public interface IPropertiesWithGet
    {
        public int Int { get; }

        public Model Model { get; }

        public string String { get; }

        public List<Model> ListOfModels { get; }
    }

    public class IPropertiesWithGetImpl : ProxyBase<IPropertiesWithGet>, IPropertiesWithGet
    {
        public IPropertiesWithGetImpl() : base(null, null)
        {
        }

        public int Int => (int)HandleMethodCall(1);

        public Model Model => throw new NotImplementedException();

        public string String => throw new NotImplementedException();

        public List<Model> ListOfModels => throw new NotImplementedException();
    }
}
