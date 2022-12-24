using System.Reflection;

namespace Overmock.Runtime
{
    /// <summary>
    /// The context for an overridden member.
    /// </summary>
    public class OverrideContext
    {
        private readonly MemberInfo _overmock;
        private readonly List<OverrideParameter> _parameters = new List<OverrideParameter>();

        /// <summary>
        /// Initializes a new instance of the <see cref="OverrideContext"/> class.
        /// </summary>
        /// <param name="overmock"></param>
        /// <param name="parameters">The parameters.</param>
        public OverrideContext(MemberInfo overmock, IEnumerable<OverrideParameter> parameters)
        {
            _overmock = overmock;
            _parameters.AddRange(parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OverrideContext"/> class.
        /// </summary>
        /// <param name="overmock"></param>
        /// <param name="parameters">The parameters.</param>
        public OverrideContext(MemberInfo overmock, params OverrideParameter[] parameters) : this(overmock, parameters.AsEnumerable())
        {
        }

        /// <summary>
        /// Gets the name of the Override.
        /// </summary>
        public string MemberName => _overmock.Name;

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