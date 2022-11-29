using System.Xml.Linq;

namespace Overmock.Setup
{
    public class OverrideContext<T> where T : class
    {
        private readonly List<OverrideParameter> _paramters = new List<OverrideParameter>();
        private readonly IOvermock<T> _parent;

        internal OverrideContext(IOvermock<T> parent, IEnumerable<OverrideParameter> parameters)
        {
            _parent = parent;
            _paramters.AddRange(parameters);
        }

        public IVerifiable<T> Parent => _parent;

        public TParameter? Get<TParameter>(string name)
        {
            var type = typeof(T);
            var param = _paramters.Find(p => p.Name == name);

            if (param == null || param.Type != typeof(TParameter))
            {
                return default;
            }

            return (TParameter)param.Value;
        }

        public TParameter? Get<TParameter>(int index)
        {
            var param = _paramters.ElementAt(index);

            if (param == null || param.Type != typeof(TParameter))
            {
                return default;
            }

            return (TParameter)param.Value;
        }
    }
    public class OverrideContext<T, TReturn> : OverrideContext<T> where T : class
    {
        internal OverrideContext(IOvermock<T> parent, IEnumerable<OverrideParameter> parameters) : base(parent, parameters)
        {
        }
    }
}