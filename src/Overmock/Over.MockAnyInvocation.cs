
namespace Overmock
{
    /// <summary>
    /// Contains methods used for configuring an overmock.
    /// </summary>
    public  static partial class Over
	{
		/// <summary>
		/// Signals the Overmock to expect any invocation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>IOvermock&lt;T&gt;.</returns>
		public static IOvermock<T> MockAnyInvocation<T>() where T : class
		{
			var result = new Overmock<T>();

			((IExpectAnyInvocation)result).ExpectAny();

			return result;
        }

        ///// <summary>
        ///// Signals the Overmock to expect any invocation.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="target">The target.</param>
        ///// <returns>IOvermock&lt;T&gt;.</returns>
        //public static T ExpectAnyInvocation<T>(T target) where T : class
        //{
        //    IOvermock<T> overmock = GetOvermock(target);

        //    ExpectAnyInvocation(overmock);

        //    return target;
        //}
    }
}