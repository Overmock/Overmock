namespace Overmock
{
    /// <summary>
    /// Represents a member that can setup a return value.
    /// </summary>
    /// <typeparam name="TReturn">The type of the t return.</typeparam>
    public interface ISetupReturnMocks<TReturn> : ISetup where TReturn : class
    {
        /// <summary>
        /// Sets up an new overmock for the return type.
        /// </summary>
        IOvermock<TReturn> ToReturnMock();

        /// <summary>
        /// Sets up an new overmock for the return type.
        /// </summary>
        IOvermock<TReturn> ToReturnMock<TMock>() where TMock : class, TReturn;

        /// <summary>
        /// Sets up an new overmock for the return type.
        /// </summary>
        IOvermock<TReturn> ToReturnMock<TMock>(IOvermock<TMock> overmock)  where TMock : class, TReturn;
    }
}
