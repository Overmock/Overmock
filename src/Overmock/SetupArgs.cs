namespace Overmock
{
    public class SetupArgs
    {
        private object[] _args = Array.Empty<object>();

        public void Args(params object[] args)
        {
            _args = args;
        }

        internal object[] Parameters => _args;
    }
}