using System.Linq.Expressions;

namespace Overmock.Mocking.Internal
{
    public class PropertyCall : MemberCall, IPropertyCall, IVerifiable
    {
        private readonly MemberExpression _expression;

        public PropertyCall(MemberExpression expression) : base((Ex.Throw.If.DeclaringTypeNull(expression.Member.DeclaringType, expression.Member.Name)))
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
        private Func<OverrideContext, TReturn>? _func;

        internal PropertyCall(MemberExpression expression) : base(expression)
        {
        }

        void IPropertyCall<TReturn>.Calls(Func<OverrideContext, TReturn> func)
        {
            _func = func;
        }
    }
}