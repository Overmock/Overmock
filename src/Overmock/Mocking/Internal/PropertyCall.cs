using System.Linq.Expressions;

namespace Overmock.Mocking.Internal
{
    public class PropertyCall<TReturn> : Verifiable<TReturn>, IPropertyCall<TReturn>
    {
        private readonly MemberExpression _expression;
        private TReturn? _return;

        internal PropertyCall(MemberExpression expression)
        {
            _expression = expression;
        }

        public MemberExpression Expression { get; }

        MemberExpression IPropertyCall<TReturn>.Expression => _expression;

        protected override void Verify()
        {
            throw new NotImplementedException();
        }

        void IPropertyCall<TReturn>.Return(TReturn value) => _return = value;
    }
}