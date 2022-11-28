using System.ComponentModel;
using System.Reflection;

namespace Overmock
{
    public interface IOvermock : IVerifiable
    {
        TMethod AddMethod<TMethod>(TMethod method) where TMethod : IMethodCall;
    }

    public interface IOvermock<T> : IVerifiable<T>, IOvermock where T : class
    {
        T Object { get; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        string TypeName { get; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetCompiledType();

        [EditorBrowsable(EditorBrowsableState.Never)]
        void SetCompiledType(Assembly assembly);

        [EditorBrowsable(EditorBrowsableState.Never)]
        IEnumerable<IMethodCall> GetOvermockedMethods();
    }
}

//public ICallAndReturn<TResult> Method<TResult>(Expression<Func<T, TResult>> expression)
//{
//    if (expression.Body is MethodCallExpression methodCall)
//    {
//        return new CallAndReturn<TResult>(
//            Overmocked.Register(this, new MethodCall<TResult>(methodCall))
//        );
//    }

//    throw new ArgumentException("Parameter must be a method call expression.");
//}

//public ICallAndReturn<P, TResult> Method<P, TResult>(Expression<Func<T, TResult>> expression)
//{
//    if (expression.Body is MethodCallExpression methodCall)
//    {
//        return new CallAndReturn<P, TResult>(
//            Overmocked.Register(this, new MethodCall<P, TResult>(methodCall))
//        );
//    }

//    throw new ArgumentException("Parameter must be a method call expression.");
//}