namespace Kimono.Internal.MethodInvokers
{
    internal sealed class Action3ObjectMethodInvoker : MethodDelegateInvoker<Action<object?, object?, object?>>
    {
        public Action3ObjectMethodInvoker(Func<Action<object?, object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            InvokeMethod(target, parameters[0], parameters[1]);

            return null;
        }
    }
}
