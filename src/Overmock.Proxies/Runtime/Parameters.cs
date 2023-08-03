using System;
using System.Collections;

namespace Overmock.Proxies
{
	public class Parameters : IReadOnlyList<object>
	{
		private readonly (RuntimeParameter Parameter, object Value)[] _parameters;

		public Parameters(RuntimeParameter[] parameters, object[] parameterValues)
		{
			_parameters = new (RuntimeParameter, object)[parameters.Length];

			for (int i = 0; i < parameters.Length; i++)
			{
				_parameters[i] = (parameters[i], parameterValues[i]);
			}
		}

		public object this[int index] => Get(index);

		public object this[string name] => Get(name);

		public object Get(int index)
		{
			return _parameters[index].Value;
		}

		public TParam Get<TParam>(int index)
		{
			return (TParam)Get(index);
		}

		public object Get(string name)
		{
			var param = Array.Find(_parameters, p => p.Parameter.Name == name);

			if (param.Value == null)
			{
				throw new IndexOutOfRangeException(name);
			}

			return param.Value;
		}

		public TParam Get<TParam>(string name)
		{
			return (TParam)Get(name);
		}

		public int Count => _parameters.Length;

		public IEnumerator<object> GetEnumerator()
		{
			yield return ToObjectArray();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		internal object[] ToObjectArray()
		{
			return _parameters.Select(p => p.Value).ToArray();
		}
	}
}