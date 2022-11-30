namespace Overmock
{
    public class OverrideContext
    {
        private readonly List<OverrideParameter> _paramters = new List<OverrideParameter>();

        public OverrideContext(IEnumerable<OverrideParameter> parameters)
        {
            _paramters.AddRange(parameters);
        }

        public OverrideContext(params OverrideParameter[] parameters) : this(parameters.AsEnumerable())
        {
            _paramters.AddRange(parameters);
        }

        public TParameter? Get<TParameter>(string name)
        {
            var param = _paramters.Find(p => p.Name == name);

            if (param == null || param.Type != typeof(TParameter))
            {
                return default;
            }

            return (TParameter)param.Value;
        }

        public TParameter? Get<TParameter>(int index)
        {
            var param = _paramters.ElementAtOrDefault(index);

            if (param == null || param.Type != typeof(TParameter))
            {
                return default;
            }

            return (TParameter)param.Value;
        }
    }
}