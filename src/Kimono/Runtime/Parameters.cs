using System.Collections;

namespace Kimono
{
	/// <summary>
	/// Represents a colleciton of method parameters.
	/// </summary>
	public class Parameters : IReadOnlyList<object>
	{
		/// <summary>
		/// The parameters
		/// </summary>
		private readonly (RuntimeParameter Parameter, object Value)[] _parameters;

		/// <summary>
		/// Initializes a new instance of the <see cref="Parameters"/> class.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		/// <param name="parameterValues">The parameter values.</param>
		public Parameters(RuntimeParameter[] parameters, object[] parameterValues)
		{
			_parameters = new (RuntimeParameter, object)[parameters.Length];

			for (int i = 0; i < parameters.Length; i++)
			{
				_parameters[i] = (parameters[i], parameterValues[i]);
			}
		}

		/// <summary>
		/// Gets the <see cref="System.Object"/> at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>System.Object.</returns>
		public object this[int index] => Get(index);

		/// <summary>
		/// Gets the <see cref="System.Object"/> with the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>System.Object.</returns>
		public object this[string name] => Get(name);

		/// <summary>
		/// Gets the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>System.Object.</returns>
		public object Get(int index)
		{
			return _parameters[index].Value;
		}

		/// <summary>
		/// Gets the specified index.
		/// </summary>
		/// <typeparam name="TParam">The type of the t parameter.</typeparam>
		/// <param name="index">The index.</param>
		/// <returns>TParam.</returns>
		public TParam Get<TParam>(int index)
		{
			return (TParam)Get(index);
		}

		/// <summary>
		/// Gets the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>System.Object.</returns>
		/// <exception cref="System.IndexOutOfRangeException"></exception>
		public object Get(string name)
		{
			var param = Array.Find(_parameters, p => p.Parameter.Name == name);

			if (param.Value == null)
			{
				throw new KeyNotFoundException(name);
			}

			return param.Value;
		}

		/// <summary>
		/// Gets the specified name.
		/// </summary>
		/// <typeparam name="TParam">The type of the t parameter.</typeparam>
		/// <param name="name">The name.</param>
		/// <returns>TParam.</returns>
		public TParam Get<TParam>(string name)
		{
			return (TParam)Get(name);
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count => _parameters.Length;

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>IEnumerator&lt;System.Object&gt;.</returns>
		public IEnumerator<object> GetEnumerator()
		{
			foreach (var item in _parameters)
			{
				yield return item.Value;
			}
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>IEnumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Converts to parameterlist.
		/// </summary>
		/// <returns>IReadOnlyList&lt;System.Object&gt;.</returns>
		internal IReadOnlyList<object> ToParameterList()
		{
			return _parameters.Select(p => p.Value).ToList();
		}

		/// <summary>
		/// Converts to array.
		/// </summary>
		/// <returns>System.Object[].</returns>
		internal object[] ToArray()
		{
			return _parameters.Select(p => p.Value).ToArray();
		}
	}
}