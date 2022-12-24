namespace Overmock.Mocking
{
    /// <summary>
    /// Represents a member that is overridden
    /// </summary>
    public interface IMemberCall : IVerifiable
    {
        /// <summary>
        /// Throws the exception when called.
        /// </summary>
        /// <param name="exception"></param>
        void Throws(Exception exception);

        /// <summary>
        /// Sets a return value to be used as the result of this operation.
        /// </summary>
        /// <param name="valueProvider"></param>
        void Returns(Func<object> valueProvider);

        /// <summary>
        /// Gets the overrides for this overmock.
        /// </summary>
        /// <returns></returns>
        IEnumerable<MemberOverride> GetOverrides();
    }
}