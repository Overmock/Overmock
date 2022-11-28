using System.Linq.Expressions;

namespace Overmock.Mocking
{
    public interface IPropertyCall<TReturn> : IVerifiable<TReturn>
    {
        MemberExpression Expression { get; }

        void Return(TReturn value);
    }
}