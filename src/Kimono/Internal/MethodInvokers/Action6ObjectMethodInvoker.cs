namespace Kimono.Internal.MethodInvokers
{
    internal sealed class Action6ObjectMethodInvoker : MethodDelegateInvoker<Action<object?, object?, object?, object?, object?, object?>>
    {
        public Action6ObjectMethodInvoker(Func<Action<object?, object?, object?, object?, object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            InvokeMethod(target, parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);

            return null;
        }
    }
}
