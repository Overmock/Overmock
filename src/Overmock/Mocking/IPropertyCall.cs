using System.Linq.Expressions;
using Overmock.Runtime;

namespace Overmock.Mocking
{
    public interface IPropertyCall : IMemberCall
    {
        MemberExpression Expression { get; }
    }

    public interface IPropertyCall<TReturn> : IPropertyCall
    {
        void Calls(Func<OverrideContext, TReturn> func);
    }
}