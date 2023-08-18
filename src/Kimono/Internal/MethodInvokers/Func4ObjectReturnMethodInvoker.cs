namespace Kimono.Internal.MethodInvokers
{
    internal sealed class Func4ObjectReturnMethodInvoker : MethodDelegateInvoker<Func<object?, object?, object?, object?, object?>>
    {
        public Func4ObjectReturnMethodInvoker(Func<object?, object?, object?, object?, object?> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            return _invokeMethod(parameters[0], parameters[1], parameters[2], parameters[3]);
        }
    }
}
