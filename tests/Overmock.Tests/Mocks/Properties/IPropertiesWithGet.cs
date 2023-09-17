using Kimono;

namespace Overmocked.Tests.Mocks.Properties
{
    public interface IPropertiesWithGet
    {
        public int Int { get; }

        public Model Model { get; }

        public string String { get; }

        public List<Model> ListOfModels { get; }
    }

    public class IPropertiesWithGetImpl : ProxyBase, IPropertiesWithGet
    {
        public IPropertiesWithGetImpl() : base(null)
        {
        }

        public int Int => (int)HandleMethodCall(1, Type.EmptyTypes, null);

        public Model Model => throw new NotImplementedException();

        public string String => throw new NotImplementedException();

        public List<Model> ListOfModels => throw new NotImplementedException();
    }
}
