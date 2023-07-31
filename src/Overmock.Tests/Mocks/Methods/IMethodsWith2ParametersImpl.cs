
namespace Overmock.Tests.Mocks.Methods
{
    public class IMethodsWith2ParametersImpl : IMethodsWith2Parameters
	{
		public bool BoolMethodWithStringAndModel(string name, Model model)
		{
			throw new NotImplementedException();
		}

		public List<Model> ListOfModelMethodWithNoParams(bool didItHappen, decimal discound)
		{
			throw new NotImplementedException();
		}

		public Model ModelMethodWithStringAndListOfModel(string name, List<Model> models)
		{
			throw new NotImplementedException();
		}

		public void VoidMethodWithIntAndString(int id, string name)
		{
			throw new NotImplementedException();
		}
	}
}