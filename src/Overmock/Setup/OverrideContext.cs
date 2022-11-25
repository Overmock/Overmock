namespace Overmock.Setup
{
    public class OverrideContext<T> where T : class
    {
        private readonly List<OverrideParameter> _paramters = new();
        private readonly IOvermock<T> _parent;

        internal OverrideContext(IOvermock<T> parent, IEnumerable<OverrideParameter> parameters)
        {
            _parent = parent;
        }

        public IVerifiable<T> Parent => _parent;

        public P? Get<P>(string name)
        {
            var type = typeof(T);
            var param = _paramters.Find(p => p.Name == name && p.Type == type);

            if (param == null)
            {
                return default;
            }

            return (P)param.Value;
        }
    }
    public class OverrideContext<T, TReturn> : OverrideContext<T> where T : class
    {
        private readonly List<OverrideParameter> _paramters = new List<OverrideParameter>();

        internal OverrideContext(IOvermock<T> parent, IEnumerable<OverrideParameter> parameters) : base(parent, parameters)
        {
        }
    }
}