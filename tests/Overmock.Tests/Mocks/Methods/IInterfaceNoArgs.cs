
namespace Overmock.Tests.Mocks.Methods
{
    public interface IInterfaceNoArgs
    {
        ImReturned Get();
    }

    public interface ImReturned
    {
        void IDoNothing();
    }

    public interface IInheritReturned : ImReturned
    {
        void IDoNothingToo();
    }
}
