namespace Overmock.Runtime
{
    /// <summary>
    /// Represents an overridden parameter.
    /// </summary>
    public class OverrideParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OverrideParameter"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        public OverrideParameter(string name, object? value = default, Type? type = default)
        {
            Type GetValueType(object? val) =>
                type ?? value?.GetType() ?? typeof(object);

            Name = name;
            Value = value!;
            Type = GetValueType(value);
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name of the parameter.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value of the parameter passed to the expected method.</value>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type of the parameter.</value>
        public Type Type { get; set; }
    }
}