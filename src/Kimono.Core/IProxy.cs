
using System;

namespace Kimono.Core
{
    public interface IProxy
    {
    }

    public interface IProxy<T> : IProxy
    {
        IInterceptor<T> Interceptor { get; }
    }
}