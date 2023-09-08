using Kimono.Core;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Emit
{
    /// <summary>
    /// Class Emitter. This class cannot be inherited.
    /// </summary>
    public abstract partial class Emitter : IEmitter
    {
        /// <inheritdoc />
        IEmitter IEmitter.BaseCtor(ConstructorInfo constructor)
        {
            _emitter.Emit(OpCodes.Call, constructor);
            return this;
        }

        /// <inheritdoc />
        IEmitter IEmitter.NewCtor(ConstructorInfo constructor)
        {
            _emitter.Emit(OpCodes.Newobj, constructor);
            return this;
        }

        IEmitter IEmitter.Invoke(MethodInfo method)
        {
            _emitter.Emit(OpCodes.Callvirt, method);
            return this;
        }

        /// <inheritdoc />
        IEmitter IEmitter.Invoke<T>(Expression<Action<T>> memberExpression)
        {
            if (memberExpression is MethodCallExpression method)
            {
                if ((method.Method.Attributes & MethodAttributes.Virtual) == MethodAttributes.Virtual)
                {
                    return ((IEmitter)this).Emit(OpCodes.Callvirt, method.Method);
                }

                return ((IEmitter)this).Invoke(method.Method);
            }

            throw new KimonoException("Invoke must be supplied a method");
        }

        /// <inheritdoc />
        IEmitter IEmitter.Invoke<T, TReturn>(Expression<Func<T, TReturn>> memberExpression)
        {
            if (memberExpression is MethodCallExpression method)
            {
                if ((method.Method.Attributes & MethodAttributes.Virtual) == MethodAttributes.Virtual)
                {
                    return ((IEmitter)this).Emit(OpCodes.Callvirt, method.Method);
                }

                return ((IEmitter)this).Invoke(method.Method);
            }

            throw new KimonoException("Invoke must be supplied a method");
        }

        /// <inheritdoc />
        ICallPropertyGetOrSet IEmitter.InvokeProperty(PropertyInfo propertyInfo)
        {
            return new PropertyGetAndSetEmitter(this, propertyInfo);
        }

        /// <inheritdoc />
        ICallPropertySet IEmitter.InvokeProperty<T>(Expression<Action<T>> memberExpression)
        {
            if (memberExpression is MemberExpression member && member.Member is PropertyInfo property)
            {
                return new PropertyGetAndSetEmitter(this, property);
            }

            throw new KimonoException("Property must be supplied a property");
        }

        /// <inheritdoc />
        ICallPropertyGet IEmitter.InvokeProperty<T, TReturn>(Expression<Func<T, TReturn>> memberExpression)
        {
            if (memberExpression is MemberExpression member && member.Member is PropertyInfo property)
            {
                return new PropertyGetAndSetEmitter(this, property);
            }

            throw new KimonoException("Property must be supplied a property");
        }
    }
}
