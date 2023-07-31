﻿
using Overmock.Runtime;

namespace Overmock.Mocking.Internal
{
	/// <summary>
	/// 
	/// </summary>
	public class ReturnProviderOverride : IOverride
	{
		internal ReturnProviderOverride(Func<object> returnProvider)
		{
			ReturnProvider = returnProvider;
		}

		/// <summary>
		/// 
		/// </summary>
		public Func<object> ReturnProvider { get; }

		public object? Handle(RuntimeContext context)
		{
			return ReturnProvider();
		}
	}
}