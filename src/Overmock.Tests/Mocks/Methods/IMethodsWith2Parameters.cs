
namespace Overmock.Tests.Mocks.Methods
{
    public interface IMethodsWith2Parameters
	{
		void VoidMethodWithIntAndString(int id, string name);

		bool BoolMethodWithStringAndModel(string name, Model model);

		Model ModelMethodWithStringAndListOfModel(string name, List<Model> models);

		List<Model> ListOfModelMethodWithNoParams(bool didItHappen, decimal discound);
	}
}