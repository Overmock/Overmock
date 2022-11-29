
namespace Overmock.Tests.Mocks
{
    public class Factory
    {
        private readonly IProvider _provider;

        public Factory(IProvider provider)
        {
            _provider = provider;
        }

        internal void GoDoWork()
        {
        }

        internal IDidWork<string> GoDoYourWork()
        {
            return new WrittenWork("internal IDidWork<string> GoDoYourWork()");
        }

        internal IDidWork<string> GoDoYourWork(object o1)
        {
            throw new NotImplementedException();
        }

        internal IDidWork<Model> GoDoYourWork(Model o1, string name)
        {
            throw new NotImplementedException();
        }
    }

    public class Factory2Ctor : Factory
    {
        public Factory2Ctor(IProvider provider) : base(provider)
        {
        }

        public new void GoDoWork()
        {
            base.GoDoWork();
        }

        public new void GoDoYourWork()
        {
            base.GoDoYourWork();
        }
    }
}