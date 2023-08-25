using System;
using System.Runtime.CompilerServices;

namespace Kimono
{
    /// <summary>
    /// Class KimonoAttribute.
    /// Implements the <see cref="CustomConstantAttribute" />
    /// </summary>
    /// <seealso cref="CustomConstantAttribute" />
    [AttributeUsage(AttributeTargets.Method)]
    public class KimonoAttribute : CustomConstantAttribute
    {
        /// <summary>
        /// The method identifier
        /// </summary>
        private readonly int _methodId;

        /// <summary>
        /// Initializes a new instance of the <see cref="KimonoAttribute"/> class.
        /// </summary>
        /// <param name="methodId">The method identifier.</param>
        public KimonoAttribute(int methodId)
        {
            _methodId = methodId;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public override object? Value => _methodId;
    }
}