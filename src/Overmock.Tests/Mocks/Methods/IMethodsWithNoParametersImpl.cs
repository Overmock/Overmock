using Overmock.Examples.Tests;
using Overmock.Runtime.Proxies;
using System.Reflection;

namespace Overmock.Tests.Mocks.Methods
{
    public class IMethodsWithNoParametersImpl : Proxy<IMethodsWithNoParameters>, IMethodsWithNoParameters
    {
        public string? Name { get; set; }

        public IMethodsWithNoParametersImpl(IOvermock target) : base(target)
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