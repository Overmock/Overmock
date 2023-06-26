namespace Overmock
{
	/// <summary>
	/// Used to generate a builder for types.
	/// </summary>
	public interface IOvermockBuilder
	{
		/// <summary>
		/// Gets the type builder.
		/// </summary>
		/// <param name="argsProvider">The arguments to use to construct the type.</param>
		/// <returns></returns>
		ITypeBuilder GetTypeBuilder(Action<SetupArgs>? argsProvider = null);
	}
}