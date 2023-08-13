using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kimono.Proxies;

namespace Kimono.Internal
{
    internal class TargetDisposer<T> : ITargetDisposer<T> where T : class, IDisposable
	{
		private bool _disposedValue;

		public TargetDisposer(T target)
		{
			Target = target;
		}

		~TargetDisposer()
		{
			Dispose(disposing: false);
		}

		public T Target { get; }

		public virtual void Dispose(bool disposing)
		{
			if (!_disposedValue && disposing)
			{
				Target.Dispose();

				_disposedValue = true;
			}
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
