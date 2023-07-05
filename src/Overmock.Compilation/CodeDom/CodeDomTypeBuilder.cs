using System;

namespace Overmock.Compilation.CodeDom
{
	/// <summary>
	/// 
	/// </summary>
	public class CodeDomTypeBuilder : ITypeBuilder
	{
		private readonly Action<SetupArgs>? _argsProvider;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="argsProvider"></param>
		public CodeDomTypeBuilder(Action<SetupArgs>? argsProvider)
		{
			_argsProvider = argsProvider;
		}

		/// <inheritdoc/>
		public T? BuildType<T>(IOvermock<T> target) where T : class
		{
			throw new NotImplementedException();
		}
	}
}
