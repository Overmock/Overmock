﻿using System.ComponentModel;
using System.Reflection;

namespace Overmock
{
	/// <summary>
	/// An interface that represents an overmocked type.
	/// </summary>
	/// <seealso cref="IVerifiable" />
	public interface IOvermock : IVerifiable
	{
		/// <summary>
		/// Adds the method.
		/// </summary>
		/// <typeparam name="TMethod">The type of the method.</typeparam>
		/// <param name="method">The method.</param>
		/// <returns>TMethod.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		TMethod AddMethod<TMethod>(TMethod method) where TMethod : IMethodCall;

		/// <summary>
		/// Adds the property.
		/// </summary>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="propertyCall">The property.</param>
		/// <returns>TProperty.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		TProperty AddProperty<TProperty>(TProperty propertyCall) where TProperty : IPropertyCall;

		/// <summary>
		/// Gets the overmocked methods.
		/// </summary>
		/// <returns>IEnumerable&lt;IMethodCall&gt;.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		IEnumerable<IMethodCall> GetOvermockedMethods();

		/// <summary>
		/// Gets the overmocked properties.
		/// </summary>
		/// <returns>IEnumerable&lt;IPropertyCall&gt;.</returns>
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