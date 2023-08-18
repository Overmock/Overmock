namespace Kimono.Internal.MethodInvokers
{
    internal sealed class Action4ObjectMethodInvoker : MethodDelegateInvoker<Action<object?, object?, object?, object?>>
    {
        public Action4ObjectMethodInvoker(Action<object?, object?, object?, object?> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            _invokeMethod(parameters[0], parameters[1], parameters[2], parameters[3]);

            return null;
        }
    }
}
