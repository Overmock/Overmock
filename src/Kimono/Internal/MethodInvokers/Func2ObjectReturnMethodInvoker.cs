namespace Kimono.Internal.MethodInvokers
{
    internal sealed class Func2ObjectReturnMethodInvoker : MethodDelegateInvoker<Func<object?, object?, object?>>
    {
        public Func2ObjectReturnMethodInvoker(Func<object?, object?, object?> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            return _invokeMethod(parameters[0], parameters[1]);
        }
    }
}
