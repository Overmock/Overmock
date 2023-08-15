
namespace Overmock.Mocking.Internal
{
    /// <summary>
    /// Class Callable.
    /// Implements the <see cref="Throwable" />
    /// Implements the <see cref="ICallable" />
    /// </summary>
    /// <seealso cref="Throwable" />
    /// <seealso cref="ICallable" />
    internal abstract class Callable : Throwable, ICallable
	{
		/// <summary>
		/// Gets the action.
		/// </summary>
		/// <value>The action.</value>
		public Action<OvermockContext>? Action { get; private set; }

		/// <summary>
		/// Gets or sets the times.
		/// </summary>
		/// <value>The times.</value>
		public Times Times { get; set; } = Times.Any;

		/// <summary>
		/// Gets the default return value.
		/// </summary>
		/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
		public abstract object? GetDefaultReturnValue();

		/// <inheritdoc />
		public void Calls(Action<OvermockContext> action, Times times)
		{
			Action = action;
			Times = times;
		}

		/// <summary>
		/// Adds the overrides to.
		/// </summary>
		/// <param name="overrides">The overrides.</param>
		protected override void AddOverridesTo(List<IOverride> overrides) 
		{
			if (Action != null)
			{
				overrides.Add(new MethodCallOverride(overmock: Action, Times));
			}

			base.AddOverridesTo(overrides);
		}
	}

    /// <summary>
    /// Class Callable.
    /// Implements the <see cref="Callable" />
    /// Implements the <see cref="ICallable{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Callable" />
    /// <seealso cref="ICallable{T}" />
    internal abstract class Callable<T> : Callable, ICallable<T>
	{
	}
}