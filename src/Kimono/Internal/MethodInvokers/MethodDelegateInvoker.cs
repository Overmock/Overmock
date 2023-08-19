namespace Kimono.Internal.MethodInvokers
{
    internal abstract class MethodDelegateInvoker<TDelegate> : IMethodDelegateInvoker where TDelegate : Delegate
    {
        private readonly Func<TDelegate> _invokeMethodProvider;

        private TDelegate? _invokeMethod;

        public MethodDelegateInvoker(Func<TDelegate> invokeMethodProvider)
        {
            _invokeMethodProvider = invokeMethodProvider;
        }

        protected TDelegate InvokeMethod => _invokeMethod ??= _invokeMethodProvider();

        public abstract object? Invoke(object? target, params object?[] parameters);
    }
}
