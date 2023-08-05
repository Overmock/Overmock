
namespace Overmock.Mocking.Internal
{
	internal abstract class Callable : Throwable, ICallable
	{
		public Action<OvermockContext>? Action { get; private set; }

		public abstract object? GetDefaultReturnValue();

		public void Calls(Action<OvermockContext> action)
		{
			Action = action;
		}

		protected override void AddOverridesTo(List<IOverride> overrides) 
		{
			if (Action != null)
			{
				overrides.Add(new MethodCallOverride(overmock: Action));
			}

			base.AddOverridesTo(overrides);
		}
	}

	internal abstract class Callable<T> : Callable, ICallable<T>
	{
	}
}