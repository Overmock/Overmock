using System.Linq.Expressions;
using Overmock.Runtime;

namespace Overmock.Mocking.Internal
{
    internal class MethodCall : MemberCall, IMethodCall
    {
        private readonly MethodCallExpression _expression;

        private Action<OverrideContext>? _method;
        //private Exception? _exception;

        internal MethodCall(MethodCallExpression expression) : base(Throw.If.DeclaringTypeNull(expression.Method.DeclaringType, expression.Method.Name))
        {
            _expression = expression;
        }

        internal MethodCallExpression Expression => _expression;

        MethodCallExpression IMethodCall.Expression => Expression;

        /// <summary>
        /// Verifies that the overmock was executed as expected
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        protected override void Verify()
        {
            throw new NotImplementedException();
        }

        void IMethodCall.Calls(Action<OverrideContext> method)
        {
            _method = method;
        }
    }

    internal class MethodCall<T> : MethodCall, IMethodCall<T> where T : class
    {
        private Action<OverrideContext>? _action;

        internal MethodCall(MethodCallExpression expression) : base(expression)
        {
        }

        protected override List<MemberOverride> GetOverrides()
        {
            var overrides = base.GetOverrides();
            
            if (_action != null)
            {
                overrides.Add(new MethodOverride(overmock: _action));
            }

            return overrides;

        }

        void IMethodCall.Calls(Action<OverrideContext> action)
        {
            _action = action;
        }
    }

    internal class MethodCall<T, TReturn> : MethodCall<T>, IMethodCall<T, TReturn> where T : class
    {
        private Func<OverrideContext, TReturn>? _func;

        internal MethodCall(MethodCallExpression expression): base(expression)
        {
        }

        void IMethodCall<T, TReturn>.Calls(Func<OverrideContext, TReturn> method)
        {
            _func = method;
        }
    }
}