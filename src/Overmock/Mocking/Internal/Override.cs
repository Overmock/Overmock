namespace Overmocked.Mocking.Internal
{
    internal abstract class Override : IOverride
    {
        public int TimesCalled { get; protected set; }

        public virtual object? Handle(OvermockContext context)
        {
            TimesCalled++;
            return HandleCore(context);
        }

        public abstract void Verify();

        protected abstract object? HandleCore(OvermockContext context);
    }
}