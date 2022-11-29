using System.Linq.Expressions;

namespace Overmock.Mocking.Internal
{
    public class PropertyCall : MemberCall, IPropertyCall, IVerifiable
    {
        private readonly MemberExpression _expression;

        public PropertyCall(MemberExpression expression) : base((Ex.Throw.IfDeclaringTypeNull(expression.Member.DeclaringType, expression.Member.Name)))
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        protected override void Verify()
        {
            throw new NotImplementedException();
        }

        MemberExpression IPropertyCall.Expression => _expression;
    }

    public class PropertyCall<TReturn> : PropertyCall, IPropertyCall<TReturn>
    {
        internal PropertyCall(MemberExpression expression) : base(expression)
        {
        }
    }
}