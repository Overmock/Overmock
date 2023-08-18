namespace Kimono.Internal.MethodInvokers
{
    internal sealed class Action3ObjectMethodInvoker : MethodDelegateInvoker<Action<object?, object?, object?>>
    {
        public Action3ObjectMethodInvoker(Action<object?, object?, object?> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            _invokeMethod(parameters[0], parameters[1], parameters[2]);

            return null;
        }
    }
}
