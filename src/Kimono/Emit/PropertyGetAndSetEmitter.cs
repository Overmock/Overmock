using System.Reflection;

namespace Kimono.Emit
{
    /// <summary>
    /// Class PropertGetAndSetEmitter.
    /// Implements the <see cref="ICallPropertyGet" />
    /// Implements the <see cref="ICallPropertySet" />
    /// </summary>
    /// <seealso cref="ICallPropertyGet" />
    /// <seealso cref="ICallPropertySet" />
    internal sealed class PropertyGetAndSetEmitter : ICallPropertyGetOrSet
    {
        private readonly IEmitter _emitter;
        private readonly PropertyInfo _property;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyGetAndSetEmitter"/> class.
        /// </summary>
        /// <param name="emitter">The emitter.</param>
        /// <param name="property"></param>
        internal PropertyGetAndSetEmitter(IEmitter emitter, PropertyInfo property)
        {
            _emitter = emitter;
            _property = property;
        }

        /// <summary>
        /// Calls the get.
        /// </summary>
        /// <returns>Kimono.Reflection.IEmitter.</returns>
        /// <exception cref="Kimono.KimonoException">Property cannot be read.</exception>
        IEmitter ICallPropertyGet.CallGet()
        {
            if (!_property.CanRead)
            {
                throw new KimonoException("Property cannot be read.");
            }

            var property = _property.GetGetMethod(true);
            _emitter.Invoke(property!);
            return _emitter;
        }

        /// <summary>
        /// Calls the set.
        /// </summary>
        /// <returns>IEmitter.</returns>
        /// <exception cref="Kimono.KimonoException">Property cannot be written.</exception>
        IEmitter ICallPropertySet.CallSet()
        {
            if (!_property.CanWrite)
            {
                throw new KimonoException("Property cannot be written.");
            }

            var property = _property.GetSetMethod(true);
            _emitter.Invoke(property!);
            return _emitter;
        }
    }

    /// <summary>
    /// Class PropertGetAndSetEmitter.
    /// Implements the <see cref="ICallPropertyGet" />
    /// Implements the <see cref="ICallPropertySet" />
    /// </summary>
    /// <seealso cref="ICallPropertyGet" />
    /// <seealso cref="ICallPropertySet" />
    internal sealed class PropertyGetAndSetEmitter<TDelegate> : ICallPropertyGetOrSet<TDelegate> where TDelegate : Delegate
    {
        private readonly IEmitter<TDelegate> _emitter;
        private readonly PropertyInfo _property;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyGetAndSetEmitter"/> class.
        /// </summary>
        /// <param name="emitter">The emitter.</param>
        /// <param name="property"></param>
        internal PropertyGetAndSetEmitter(IEmitter<TDelegate> emitter, PropertyInfo property)
        {
            _emitter = emitter;
            _property = property;
        }

        /// <summary>
        /// Calls the get.
        /// </summary>
        /// <returns>Kimono.Reflection.IEmitter.</returns>
        /// <exception cref="Kimono.KimonoException">Property cannot be read.</exception>
        IEmitter<TDelegate> ICallPropertyGet<TDelegate>.CallGet()
        {
            if (!_property.CanRead)
            {
                throw new KimonoException("Property cannot be read.");
            }

            var property = _property.GetGetMethod(true);
            _emitter.Invoke(property!);
            return _emitter;
        }

        /// <summary>
        /// Calls the set.
        /// </summary>
        /// <returns>IEmitter&lt;TDelegate&gt;.</returns>
        /// <exception cref="Kimono.KimonoException">Property cannot be written.</exception>
        IEmitter<TDelegate> ICallPropertySet<TDelegate>.CallSet()
        {
            if (!_property.CanWrite)
            {
                throw new KimonoException("Property cannot be written.");
            }

            var property = _property.GetSetMethod(true);
            _emitter.Invoke(property!);
            return _emitter;
        }
    }
}
