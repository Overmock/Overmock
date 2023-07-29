using Overmock.Runtime;

namespace Overmock.Mocking.Internal
{
	internal class Returnable<T, TReturn> : Callable, IReturnable<TReturn>
	{
		public Func<RuntimeContext, TReturn>? Func { get; private set; }

		public void Calls(Func<RuntimeContext, TReturn> func)
		{
			Func = func;
		}

		public void Returns(TReturn value)
		{
			Returns(() => value);
		}

		public void Returns(Func<TReturn> func)
		{
			Func = c => func();
		}

		protected override void AddOverridesTo(List<IOverride> overrides)
		{
			if (Func != null)
			{
				overrides.Add(new MethodCallOverride(overmock: Func));
			}

			base.AddOverridesTo(overrides);
		}
	}
}