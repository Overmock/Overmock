namespace Kimono.Internal.MethodInvokers
{
    internal sealed class ActionMethodInvoker : MethodDelegateInvoker<Action>
    {
        public ActionMethodInvoker(Action invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            _invokeMethod();

            return null;
        }
    }
}
