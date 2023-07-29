namespace Overmock.Mocking.Internal
{

	internal abstract class Throwable : Overridable, IThrowable
	{
		public Exception? Exception { get; private set; }

		public void Throws(Exception exception)
		{
			Exception = exception;
		}

		protected override void AddOverridesTo(List<IOverride> overrides)
		{
			if (Exception != null)
			{
				overrides.Add(new ThrowExceptionOverride(Exception));
			}
		}
	}
}