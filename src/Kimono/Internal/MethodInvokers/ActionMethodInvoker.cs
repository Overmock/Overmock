namespace Kimono.Internal.MethodInvokers
{
    internal sealed class ActionMethodInvoker : MethodDelegateInvoker<Action>
    {
        public ActionMethodInvoker(Func<Action> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            InvokeMethod();

            return null;
        }
    }
}
