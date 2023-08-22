using System;

namespace Kimono
{
	/// <summary>
	/// Represents an overridden parameter.
	/// </summary>
	public class RuntimeParameter
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeParameter" /> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		public RuntimeParameter(string name, Type type)
		{
			Name = name;
			Type = type;
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name of the parameter.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>The type of the parameter.</value>
		public Type Type { get; set; }
	}
}