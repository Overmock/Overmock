namespace Overmock.Runtime.Expectations
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConstraint
    {
        /// <summary>
        /// 
        /// </summary>
        bool WasCalled { get; }
    }

    /// <summary>
    /// Base class for all constraints.
    /// </summary>
    public abstract class Constraint : IConstraint
    {
        private bool _wasCalled;

        /// <summary>
        /// 
        /// </summary>
        protected Constraint()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        bool IConstraint.WasCalled => _wasCalled;

        internal virtual bool WasCalled
        {
            get
            {
                return _wasCalled;
            }
            set
            {
                _wasCalled = value;
            }
        }
    }
}
