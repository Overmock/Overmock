
namespace Overmocked.Tests.Mocks.Methods
{
    public interface IMethodsWith1Parameter
    {
        bool BoolMethodWithString(string name);

        List<Model> ListOfModelMethodWithDecimal(decimal discound);

        Model ModelMethodWithFuncOfListOfModel(Func<List<Model>> modelsProvider);

        Model ModelMethodWithFuncOfListOfModel(int id);

        void VoidMethodWithInt(int id);
    }
}