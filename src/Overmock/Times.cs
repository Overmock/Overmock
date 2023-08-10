
using System.Diagnostics;
using System.Globalization;

namespace Overmock
{
	/// <summary>
	/// 
	/// </summary>
	[DebuggerDisplay(null, Name = "Value")]
	public readonly struct Times
	{
		private readonly int _times = Any;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="times"></param>
		public Times(int times)
		{
			_times = times;
		}

		/// <summary>
		/// 
		/// </summary>
		public readonly static Times Once = new Times(1);

		/// <summary>
		/// 
		/// </summary>
		public readonly static Times Zero = new Times(0);

		/// <summary>
		/// 
		/// </summary>
		public readonly static Times Any = new Times(-1);

		/// <summary>
		/// 
		/// </summary>
		public int Value => _times;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="times"></param>
		public static implicit operator int(Times times)
		{
			return times._times;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="times"></param>
		public static implicit operator Times(int times)
		{
			return times == 1
				? Once : times == 0
					? Zero : times < 0
						? Any : new Times(times);
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return _times.ToString(CultureInfo.CurrentCulture);
		}

		internal void ThrowIfInvalid(int count)
		{
			// -1 represents inifinity.
			if (_times == -1)
			{
				return;
			}

			if (count > _times)
			{
				throw new OvermockException($"Method called: Expected: {_times}, Actual: {count}");
			}
		}
	}
}
