namespace Kimono
{
	internal class InvocationChainBuilder : IInvocationChainBuilder
	{
		public IInvocationHandler Build()
		{
			return new InvocationChainHandler();
		}

		private class InvocationChainHandler : IInvocationHandler
		{
			public void Handle(IInvocationContext context)
			{
			}
		}
	}
}