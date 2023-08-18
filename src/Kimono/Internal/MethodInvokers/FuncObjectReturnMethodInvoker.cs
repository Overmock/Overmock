namespace Kimono.Internal.MethodInvokers
{
    internal sealed class FuncObjectReturnMethodInvoker : MethodDelegateInvoker<Func<object?, object?>>
    {
        public FuncObjectReturnMethodInvoker(Func<object?, object?> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            return _invokeMethod(parameters[0]);
        }
    }
}
