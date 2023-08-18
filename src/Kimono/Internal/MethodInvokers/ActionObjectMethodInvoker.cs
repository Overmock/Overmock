namespace Kimono.Internal.MethodInvokers
{
    internal sealed class ActionObjectMethodInvoker : MethodDelegateInvoker<Action<object?>>
    {
        public ActionObjectMethodInvoker(Action<object?> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            _invokeMethod(parameters[0]);

            return null;
        }
    }
}
