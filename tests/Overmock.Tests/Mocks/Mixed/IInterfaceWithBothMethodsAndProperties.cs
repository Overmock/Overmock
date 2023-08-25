namespace Overmock.Tests.Mocks.Mixed
{
    public interface IInterfaceWithBothMethodsAndProperties
    {
        string Name { get; }

        void DoSomething(string name);

        string MethodWithReturn(string name);
    }
}
