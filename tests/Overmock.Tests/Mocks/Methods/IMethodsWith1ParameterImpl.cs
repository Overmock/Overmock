
using Kimono;
using Kimono.Proxies;

namespace Overmocked.Tests.Mocks.Methods
{
    public class IMethodsWith1ParameterImpl : ProxyBase<IMethodsWith1Parameter>, IMethodsWith1Parameter
    {
        public IMethodsWith1ParameterImpl(ProxyContext proxyContext, IInterceptor interceptor) : base(proxyContext, interceptor)
        {
        }

        public bool BoolMethodWithString(string name)
        {
            throw new NotImplementedException();
        }

        public List<Model> ListOfModelMethodWithDecimal(decimal discound)
        {
            throw new NotImplementedException();
        }

        public Model ModelMethodWithFuncOfListOfModel(Func<List<Model>> modelsProvider)
        {
            throw new NotImplementedException();
        }

        public Model ModelMethodWithFuncOfListOfModel(int id)
        {
            return (Model)(HandleMethodCall(52, Type.EmptyTypes, new object[] { id }));
        }

        public void VoidMethodWithInt(int id)
        {
            throw new NotImplementedException();
        }
    }
}