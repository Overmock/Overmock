namespace Overmock.Mocking.Internal
{
	/// <summary>
	/// Class Returnable.
	/// Implements the <see cref="Overmock.Mocking.Internal.Callable" />
	/// Implements the <see cref="Overmock.Mocking.IReturnable{TReturn}" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TReturn">The type of the t return.</typeparam>
	/// <seealso cref="Overmock.Mocking.Internal.Callable" />
	/// <seealso cref="Overmock.Mocking.IReturnable{TReturn}" />
	internal abstract class Returnable<T, TReturn> : Callable, IReturnable<TReturn>
	{
		/// <summary>
		/// Gets the function.
		/// </summary>
		/// <value>The function.</value>
		public Func<OvermockContext, TReturn>? Func { get; private set; }

		/// <inheritdoc />
		public override object? GetDefaultReturnValue()
		{
			return default(TReturn);
		}

		/// <summary>
		/// Callses the specified function.
		/// </summary>
		/// <param name="func">The function.</param>
		/// <param name="times">The times.</param>
		public void Calls(Func<OvermockContext, TReturn> func, Times times)
		{
			Func = func;
			Times = times;
		}

		/// <summary>
		/// Returnses the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		public void Returns(TReturn value)
		{
			Returns(() => value);
		}

		/// <summary>
		/// Returnses the specified function.
		/// </summary>
		/// <param name="func">The function.</param>
		public void Returns(Func<TReturn> func)
		{
			Func = c => func();
		}

		/// <inheritdoc />
		protected override void AddOverridesTo(List<IOverride> overrides)
		{
			if (Func != null)
			{
				overrides.Add(new MethodCallOverride(overmock: Func, Times));
			}

			base.AddOverridesTo(overrides);
		}
	}
}