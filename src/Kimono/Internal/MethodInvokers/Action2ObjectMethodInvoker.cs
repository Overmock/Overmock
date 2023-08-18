namespace Kimono.Internal.MethodInvokers
{
    internal sealed class Action2ObjectMethodInvoker : MethodDelegateInvoker<Action<object?, object?>>
    {
        public Action2ObjectMethodInvoker(Action<object?, object?> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            _invokeMethod(parameters[0], parameters[1]);

            return null;
        }
    }
}
