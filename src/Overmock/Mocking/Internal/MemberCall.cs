namespace Overmock.Mocking.Internal
{
    public abstract class MemberCall : Verifiable, IMemberCall
    {
        private Exception? _exception;
        private Func<object>? _valueProvider;

        protected MemberCall(Type type) : base(type)
        {
        }

        protected virtual List<MemberOverride> GetOverrides()
        {
            var overrides = new List<MemberOverride>();

            if (_exception != null)
            {
                overrides.Add(new MethodOverride(exception: _exception));
            }

            if (_valueProvider != null)
            {
                overrides.Add(new MemberOverride(returnProvider: _valueProvider));
            }

            return overrides;
        }

        void IMemberCall.Returns(Func<object> valueProvider)
        {
            _valueProvider = valueProvider;
        }

        IEnumerable<MemberOverride> IMemberCall.GetOverrides()
        {
            return GetOverrides().AsReadOnly();
        }

        void IMemberCall.Throws(Exception exception)
        {
            _exception = exception;
        }
    }
}