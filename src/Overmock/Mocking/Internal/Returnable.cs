
namespace Overmock.Mocking.Internal
{
	internal abstract class Returnable<T, TReturn> : Callable, IReturnable<TReturn>
	{
		public Func<OvermockContext, TReturn>? Func { get; private set; }

		public override object? GetDefaultReturnValue()
		{
			return default(TReturn);
		}

		public void Calls(Func<OvermockContext, TReturn> func, Times times)
		{
			Func = func;
			Times = times;
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
				overrides.Add(new MethodCallOverride(overmock: Func, Times));
			}

			base.AddOverridesTo(overrides);
		}
	}
}