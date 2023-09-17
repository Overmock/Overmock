using System;

namespace Kimono
{
    /// <summary>
    /// Interface IDisposableInterceptor
    /// Extends the <see cref="Kimono.IInterceptor" />
    /// Extends the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="Kimono.IInterceptor" />
    /// <seealso cref="System.IDisposable" />
    public interface IDisposableInterceptor : IInterceptor, IDisposable
    {
    }
}
