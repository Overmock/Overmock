namespace Kimono
{
	internal sealed class InvocationChainBuilder : IInvocationChainBuilder
	{
		private readonly List<InvocationChainAction> _invocationChain = new();

		public IInvocationChainBuilder Add(InvocationChainAction  action)
		{
			_invocationChain.Add(action);
			return this;
		}

		public IInvocationChainBuilder Add(IInvocationChainHandler handler)
		{
			return Add(handler.Handle);
		}

		public IInvocationHandler Build()
		{
			return new InvocationChainHandler(_invocationChain);
		}

		private sealed class InvocationChainHandler : IInvocationHandler
		{
			private readonly IReadOnlyList<InvocationChainAction> _chain;

			public InvocationChainHandler(IEnumerable<InvocationChainAction> handlers)
			{
				_chain = handlers.ToList();
			}

			public void Handle(IInvocationContext context)
			{
				using var enumerator = _chain.GetEnumerator();

				void Next(IInvocationContext context)
				{
					if (enumerator.MoveNext())
					{
						enumerator.Current.Invoke(Next, context);
					}
				}

				Next(context);
			}
		}
	}
}