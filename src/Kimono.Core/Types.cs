using System;
using System.Reflection;

namespace Kimono
{
    internal static class Types
    {
        public static readonly Type Object = typeof(object);

        public static readonly Type Disposable = typeof(IDisposable);

        public static readonly Type[] SingleObjectArray = new[] { Types.Object };
        public static readonly Type Void = typeof(void);

        /// <summary>
        /// The array type
        /// </summary>
        public static readonly Type Array = typeof(Array);

        /// <summary>
        /// The method information type
        /// </summary>
        public static readonly Type MethodInfoType = typeof(MethodInfo);
        
        /// <summary>
        /// The type type
        /// </summary>
        public static readonly Type Type = typeof(Type);

        /// <summary>
        /// The array type
        /// </summary>
        public static readonly Type TypeArray = typeof(Type[]);

        /// <summary>
        /// The Generic <see cref="ProxyBase"/>
        /// </summary>
        public static readonly Type ProxyBaseNonGeneric = typeof(ProxyBase);

        /// <summary>
        /// The Generic <see cref="ProxyBase{}"/>
        /// </summary>
        public static readonly Type ProxyBase = typeof(ProxyBase<>);

        /// <summary>
        /// The kimono context type
        /// </summary>
        public static Type IInterceptorType<T>() => typeof(IInterceptor<T>);

        /// <summary>
        /// The proxy type
        /// </summary>
        public static Type[] ProxyBaseCtorParameterTypes<T>() => new Type[]
        {
                IInterceptorType<T>()
        };
        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Action1ObjectType = typeof(Action<object?>);
        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Action2ObjectType = typeof(Action<object?, object?>);
        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Action3ObjectType = typeof(Action<object?, object?, object?>);
        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Action4ObjectType = typeof(Action<object?, object?, object?, object?>);
        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Action5ObjectType = typeof(Action<object?, object?, object?, object?, object?>);
        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Action6ObjectType = typeof(Action<object?, object?, object?, object?, object?, object?>);
        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Func1ObjectType = typeof(Func<object?, object?>);
        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Func2ObjectType = typeof(Func<object?, object?, object?>);
        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Func3ObjectType = typeof(Func<object?, object?, object?, object?>);
        /// <summary>
        /// The func type taking 3 objects.
        /// </summary>
        public static readonly Type Func4ObjectType = typeof(Func<object?, object?, object?, object?, object?>);
        /// <summary>
        /// The func type taking 5 objects.
        /// </summary>
        public static readonly Type Func5ObjectType = typeof(Func<object?, object?, object?, object?, object?, object?>);
        /// <summary>
        /// The func type taking 6 objects.
        /// </summary>
        public static readonly Type Func6ObjectType = typeof(Func<object?, object?, object?, object?, object?, object?, object?>);
        /// <summary>
        /// The <see cref="Func{ProxyContext, IInterceptor, T}"/> type
        /// </summary>
        public static Type GetFuncProxyContextIInterceptorTType<T>() => typeof(Func<IInterceptor<T>, T>);

        /// <summary>
        /// 
        /// </summary>
        public static readonly MethodInfo GetTypeFromHandleMethod = Type.GetMethod(nameof(Type.GetTypeFromHandle), BindingFlags.Static | BindingFlags.Public)!;
    }
}
