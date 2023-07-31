﻿using Overmock.Runtime;

namespace Overmock.Mocking.Internal
{
	internal class ValueOverride : IOverride
	{
		public ValueOverride(object value)
		{
			Value = value;
		}

		public object Value { get; }

		public object? Handle(RuntimeContext context)
		{
			return Value;
		}
	}
}