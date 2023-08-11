using System;
using System.Collections;
using System.Collections.Generic;

namespace Kimono
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
			foreach (var item in _parameters)
			{
				yield return item.Value;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		internal IReadOnlyList<object> ToParameterList()
		{
			return _parameters.Select(p => p.Value).ToList();
		}

		internal object[] ToArray()
		{
			return _parameters.Select(p => p.Value).ToArray();
		}
	}
}