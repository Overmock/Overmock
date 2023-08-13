namespace Kimono
{
	/// <summary>
	/// Delegate InvocationAction
	/// </summary>
	/// <param name="next"></param>
	/// <param name="context">The context.</param>
	public delegate void InvocationAction(IInvocationContext context);

	/// <summary>
	/// Delegate InvocationAction
	/// </summary>
	/// <param name="next"></param>
	/// <param name="context">The context.</param>
	public delegate void InvocationChainAction(InvocationAction next, IInvocationContext context);

	internal class InvocationChainBuilder : IInvocationChainBuilder
	{
		private readonly List<InvocationChainAction> _invocationChain = new List<InvocationChainAction>();

		public IInvocationChainBuilder Add(InvocationChainAction  action)
		{
			_invocationChain.Add(action);
			return this;
		}

		public IInvocationHandler Build()
		{
			return new InvocationChainHandler(_invocationChain);
		}

		private class InvocationChainHandler : IInvocationHandler
		{
			private readonly IReadOnlyList<InvocationChainAction> _chain;

			public InvocationChainHandler(IEnumerable<InvocationChainAction> handlers)
			{
				_chain = handlers.ToList();
			}

			public void Handle(IInvocationContext context)
			{
				var enumerator = _chain.GetEnumerator();
				enumerator.MoveNext();

				InvocationAction next;

				var first = new InvocationAction(c =>
				{
					var current = enumerator.Current;

					void Next(IInvocationContext context)
					{
						if (enumerator.MoveNext())
						{
							next = Next;
						}
					}
					
					enumerator.Current.Invoke(Next, context);
				});
			}
		}
	}
}