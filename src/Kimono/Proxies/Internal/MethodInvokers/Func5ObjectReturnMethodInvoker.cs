namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal sealed class Func5ObjectReturnMethodInvoker : MethodDelegateInvoker<Func<object?, object?, object?, object?, object?, object?>>
    {
        public Func5ObjectReturnMethodInvoker(Func<Func<object?, object?, object?, object?, object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            return InvokeMethod(target, parameters[0], parameters[1], parameters[2], parameters[3]);
        }
    }
}
