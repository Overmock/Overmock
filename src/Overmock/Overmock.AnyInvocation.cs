
namespace Overmocked
{
    /// <summary>
    /// Contains methods used for configuring an overmock.
    /// </summary>
    public static partial class Overmock
    {
        /// <summary>
        /// Signals the Overmock to expect any invocation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IOvermock&lt;T&gt;.</returns>
        public static T AnyInvocation<T>() where T : class
        {
            var result = new Overmock<T>();

            ((IExpectAnyInvocation)result).ExpectAny();

            return result;
        }

        /// <summary>
        /// Mocks the given interface type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Interface<T>() where T : class
        {
            return new Overmock<T>();
        }
    }
}