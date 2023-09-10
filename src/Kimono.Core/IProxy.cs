
namespace Kimono.Core
{
    public interface IProxy
    {

    }

    public interface IProxy<T> : IProxy
    {
        IInterceptor<T> Interceptor { get; }
    }

    public abstract class ProxyBase : IProxy
    {
        protected ProxyBase() { }
    }

    public abstract class ProxyBase<T> : IProxy<T>
    {
        private readonly IInterceptor<T> _interceptor;
        protected ProxyBase(IInterceptor<T> interceptor)
        {
            _interceptor = interceptor;
        }

        public IInterceptor<T> Interceptor => _interceptor;

        protected object? HandleMethodCall(int methodId, object[] parameters)
        {
            return _interceptor.HandleInvocation(methodId, this, parameters);
        }
    }
}