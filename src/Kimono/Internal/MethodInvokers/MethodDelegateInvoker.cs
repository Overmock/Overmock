namespace Kimono.Internal.MethodInvokers
{
    internal abstract class MethodDelegateInvoker<TDelegate> : IMethodDelegateInvoker where TDelegate : Delegate
    {
        protected readonly TDelegate _invokeMethod;

        public MethodDelegateInvoker(TDelegate invokeMethod)
        {
            _invokeMethod = invokeMethod;
        }

        public abstract object? Invoke(object? target, params object?[] parameters);
    }
}
