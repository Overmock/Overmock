namespace Kimono.Internal.MethodInvokers
{
    internal sealed class Func3ObjectReturnMethodInvoker : MethodDelegateInvoker<Func<object?, object?, object?, object?>>
    {
        public Func3ObjectReturnMethodInvoker(Func<object?, object?, object?, object?> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            return _invokeMethod(parameters[0], parameters[1], parameters[2]);
        }
    }
}
