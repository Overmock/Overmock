using System.Linq.Expressions;

namespace Overmock.Mocking
{
    public interface IMethodCall : IMemberCall
    {
        MethodCallExpression Expression { get; }

        void Call(Action<OverrideContext> method);
    }

    public interface IMethodCall<T> : IMethodCall where T : class
    {
    }

    public interface IMethodCall<T, TReturn> : IMethodCall<T> where T : class
    {
        void Call(Func<OverrideContext, TReturn> method);
    }
}