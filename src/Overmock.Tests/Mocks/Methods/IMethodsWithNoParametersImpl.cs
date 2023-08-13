using Kimono;

namespace Overmock.Tests.Mocks.Methods
{
    public class IMethodsWithNoParametersImpl : ProxyBase<IMethodsWithNoParameters>, IMethodsWithNoParameters
    {
        public string Name { get; set; }

        public IMethodsWithNoParametersImpl() : base()
        {
        }

        public void VoidMethodWithNoParams()
        {
            throw new NotImplementedException();
        }

        public bool BoolMethodWithNoParams()
        {
            throw new NotImplementedException();
        }

        public Model ModelMethodWithNoParams()
        {
            throw new NotImplementedException();
        }

        public List<Model> ListOfModelMethodWithNoParams()
        {
            throw new NotImplementedException();
        }
    }
}