namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal sealed class Action4ObjectMethodInvoker : MethodDelegateInvoker<Action<object?, object?, object?, object?>>
    {
        public Action4ObjectMethodInvoker(Func<Action<object?, object?, object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            InvokeMethod(target, parameters[0], parameters[1], parameters[2]);

            return null;
        }
    }
}
