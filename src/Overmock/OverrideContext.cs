namespace Overmock
{
    /// <summary>
    /// The context for an overridden member.
    /// </summary>
    public class OverrideContext
    {
        private readonly List<OverrideParameter> _parameters = new List<OverrideParameter>();

        /// <summary>
        /// Initializes a new instance of the <see cref="OverrideContext"/> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public OverrideContext(IEnumerable<OverrideParameter> parameters)
        {
            _parameters.AddRange(parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OverrideContext"/> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public OverrideContext(params OverrideParameter[] parameters) : this(parameters.AsEnumerable())
        {
            _parameters.AddRange(parameters);
        }

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public TParameter? Get<TParameter>(string name)
        {
            var param = _parameters.Find(p => p.Name == name);

            if (param == null || param.Type != typeof(TParameter))
            {
                return default;
            }

            return (TParameter)param.Value;
        }

        /// <summary>
        /// Gets the specified index.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public TParameter? Get<TParameter>(int index)
        {
            var param = _parameters.ElementAtOrDefault(index);

            if (param == null || param.Type != typeof(TParameter))
            {
                return default;
            }

            return (TParameter)param.Value;
        }
    }
}