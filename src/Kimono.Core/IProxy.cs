
using System;

namespace Kimono
{
    public interface IProxy
    {
    }

    public interface IProxy<T> : IProxy
    {
        IInterceptor<T> Interceptor { get; }
    }
}