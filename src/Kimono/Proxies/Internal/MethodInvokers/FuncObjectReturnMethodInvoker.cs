namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal sealed class FuncObjectReturnMethodInvoker : MethodDelegateInvoker<Func<object?, object?>>
    {
        public FuncObjectReturnMethodInvoker(Func<Func<object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            return InvokeMethod(target);
        }
    }
}
