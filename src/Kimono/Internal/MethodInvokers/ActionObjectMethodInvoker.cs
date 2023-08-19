namespace Kimono.Internal.MethodInvokers
{
    internal sealed class ActionObjectMethodInvoker : MethodDelegateInvoker<Action<object?>>
    {
        public ActionObjectMethodInvoker(Func<Action<object?>> invokeProvider) : base(invokeProvider)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            InvokeMethod(target);

            return null;
        }
    }
}
