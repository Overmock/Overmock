using System.ComponentModel;
using System.Reflection;

namespace Overmock
{
	/// <summary>
	/// An interface that represents an overmocked type.
	/// </summary>
	/// <seealso cref="Overmock.Mocking.IVerifiable" />
	public interface IOvermock : IVerifiable
	{
		/// <summary>
		/// Adds the method.
		/// </summary>
		/// <typeparam name="TMethod">The type of the method.</typeparam>
		/// <param name="method">The method.</param>
		/// <returns></returns>
		TMethod AddMethod<TMethod>(TMethod method) where TMethod : IMethodCall;

		/// <summary>
		/// Adds the property.
		/// </summary>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="propertyCall">The property.</param>
		/// <returns></returns>
		TProperty AddProperty<TProperty>(TProperty propertyCall) where TProperty : IPropertyCall;

		/// <summary>
		/// Gets the name of the type.
		/// </summary>
		/// <value>
		/// The name of the type.
		/// </value>
		[EditorBrowsable(EditorBrowsableState.Never)]
		string TypeName { get; }

		/// <summary>
		/// Gets the type of the compiled.
		/// </summary>
		/// <returns></returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		Type? GetCompiledType();

		/// <summary>
		/// Sets the type of the compiled mock.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		void SetCompiledType(Assembly assembly);

		/// <summary>
		/// Sets the type of the compiled mock.
		/// </summary>
		/// <param name="compiledType">The compiled Type.</param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		void SetCompiledType(Type compiledType);

		/// <summary>
		/// Gets the overmocked methods.
		/// </summary>
		/// <returns></returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		IEnumerable<IMethodCall> GetOvermockedMethods();

		/// <summary>
		/// Gets the overmocked properties.
		/// </summary>
		/// <returns></returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		IEnumerable<IPropertyCall> GetOvermockedProperties();
	}

	/// <summary>
	/// Represents a mocked type who's members can be overridden.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Overmock.Mocking.IVerifiable" />
	public interface IOvermock<T> : IVerifiable<T>, IOvermock where T : class
	{
		/// <summary>
		/// Gets the mocked object.
		/// </summary>
		/// <value>The mocked object.</value>
		T Target { get; }
	}
}