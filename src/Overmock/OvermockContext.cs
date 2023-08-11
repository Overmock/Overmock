﻿using Kimono;

namespace Overmock
{
	/// <summary>
	/// 
	/// </summary>
	public class OvermockContext
	{
		private readonly InvocationContext _invocationContext;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="invocationContext"></param>
		public OvermockContext(InvocationContext invocationContext)
		{
			_invocationContext = invocationContext;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public T Get<T>(string name)
		{
			return _invocationContext.Parameters.Get<T>(name);
		}

		/// <summary>
		/// 
		/// </summary>
		public object? ReturnValue
		{
			get => _invocationContext.ReturnValue;
			set => _invocationContext.ReturnValue = value;
		}
	}
}
