using System.Linq.Expressions;

namespace Overmock.Verification
{
    public interface IPropertyCall<TReturn> : IVerifiable<TReturn>
    {
        MemberExpression Expression { get; }

        void Return(TReturn value);
    }
}