namespace Overmock
{
    public record SetupArgs
    {
        private static readonly object[] DefaultValue = Array.Empty<object>();

        private object _args = new object[0];

        public object Args
        {
            get
            {
                return _args;
            }
            set
            {
                _args = value ?? DefaultValue;
            }
        }
    }
}