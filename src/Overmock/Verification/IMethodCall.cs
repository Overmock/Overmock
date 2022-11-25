using System.Linq.Expressions;

namespace Overmock
{
    public interface IMethodCall : IVerifiable
    {
        MethodCallExpression Expression { get; }

        void Call(Action method);

        void Throws(Exception exception);
    }

    public interface IMethodCall<T> : IMethodCall where T : class
    {
        void Call(Action<OverrideContext<T>> method);
    }

    public interface IMethodCall<T, TReturn> : IMethodCall<T> where T : class
    {
        void Call(Action<OverrideContext<T, TReturn>> method);

        void Call(Func<OverrideContext<T, TReturn>, TReturn> method);
    }
}