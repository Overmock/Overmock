using Kimono.Internal;
using Kimono.Msil;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ProxyFactory : IProxyFactory
    {
        static ProxyFactory()
        {
            var assemblyName = new AssemblyName(Names.DllName);

            Assembly = AssemblyBuilder.DefineDynamicAssembly(
                assemblyName,
                AssemblyBuilderAccess.RunAndCollect);

            Module = Assembly.DefineDynamicModule(Names.ModuleName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delegateFactory"></param>
        /// <param name="cache"></param>
        public ProxyFactory(IDelegateFactory delegateFactory, IProxyGeneratorCache cache)
        {
            MethodFactory = delegateFactory;
            Cache = cache;
        }

        private static AssemblyBuilder Assembly { get; }

        private static ModuleBuilder Module { get; }

        /// <summary>
        /// 
        /// </summary>
        public IDelegateFactory MethodFactory { get; }

        /// <summary>
        /// 
        /// </summary>
        public IProxyGeneratorCache Cache { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        public static IProxyFactory Create(IDelegateFactory? factory = null, IProxyGeneratorCache? cache = null)
        {
            return new ProxyFactory(
                factory ?? DelegateFactory.Current,
                cache ?? ProxyCache.Current
            );
        }

        /// <inheritdoc />
        public T CreateInterfaceProxy<T>(IInterceptor<T> interceptor) where T : class
        {
            var generator = Cache.GetGenerator<T>();

            if (generator == null)
            {
                generator = Cache.SetGenerator(
                    new LazyProxyGenerator<T>(() => CreateProxyGenerator(interceptor))
                );
            }

            return generator.GenerateProxy(interceptor);
        }

        /// <inheritdoc/>
        public T CreateInterfaceProxy<T>(IInterceptorBuilder builder) where T : class
        {
            return CreateInterfaceProxy(builder.Build<T>());
        }

        /// <inheritdoc />
        public T CreateInterfaceProxy<T>(Action<IInvocation> callback) where T : class
        {
            return CreateInterfaceProxy<T>(new InterceptorBuilder()
                .AddCallback((next, invocation) => {
                    callback.Invoke(invocation);
                    next(invocation);
                }).Build<T>());
        }

        /// <summary>
        /// Creates a proxy generator for the specified interface type.
        /// </summary>
        /// <remarks>
        /// This is the main entry point for creating interface proxies. It:
        /// 1. Creates a new type that inherits from ProxyBase and implements the target interface
        /// 2. Implements all methods and properties from the interface (and parent interfaces)
        /// 3. Generates IL for each method that forwards calls to the interceptor
        /// 4. Creates a delegate factory for instantiating the proxy
        ///
        /// The generated proxy type is cached, so subsequent calls for the same interface
        /// will reuse the previously generated type.
        ///
        /// The proxy generation process involves:
        /// - Dynamic type creation using TypeBuilder
        /// - Method/property implementation via MethodBuilder
        /// - IL generation for method forwarding
        /// - Constructor generation for interceptor injection
        /// </remarks>
        /// <typeparam name="T">The interface type to create a proxy for.</typeparam>
        /// <param name="interceptor">The interceptor that will handle method calls.</param>
        /// <returns>A proxy generator that can create instances of the proxy type.</returns>
        /// <exception cref="KimonoException">Thrown if the proxy type creation fails.</exception>
        public IProxyGenerator<T> CreateProxyGenerator<T>(IInterceptor<T> interceptor) where T : class
        {
            const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

            var targetType = typeof(T);
            var proxyBaseType = Types.ProxyBaseNonGeneric;
            var methodId = MethodId.Create();

            // Create a new type that inherits from ProxyBase and implements the target interface
            var typeBuilder = Module.DefineType(
                string.Format(CultureInfo.CurrentCulture, Names.TypeName, targetType.Name),
                TypeAttributes.Public | TypeAttributes.Sealed,
                proxyBaseType);

            // Find the ProxyBase constructor that takes an IInterceptor
            var ctorParameters = Types.ProxyBaseCtorParameterTypes;
            var baseConstructor = proxyBaseType.GetConstructor(
                bindingFlags, null,
                ctorParameters,
                null
            )!;

            // Build all methods and properties with their IL implementations
            var metadatas = BuildTypeMetadata(
                interceptor,
                targetType,
                methodId,
                typeBuilder,
                ctorParameters,
                baseConstructor
            );

            // Finalize the type - this completes the type building process
            var proxyType = typeBuilder.CreateType() ?? throw new KimonoException($"Failed to create proxy type for: {targetType}");

            // Create a generator that can instantiate the proxy with an interceptor
            return new ProxyGenerator<T>(ProxyContext.Create(metadatas),
                MethodFactory.CreateProxyConstructorDelegate<T>(
                    proxyType,
                    targetType,
                    proxyType.GetConstructor(ctorParameters)!
                )
            );
        }

        private MethodMetadata[] BuildTypeMetadata<T>(IInterceptor<T> interceptor, Type targetType, MethodId methodId, TypeBuilder typeBuilder, Type[] ctorParameters, ConstructorInfo baseConstructor) where T : class
        {
            var (methods, properties) = AddInterfaceImplementations(typeBuilder, targetType);

            ImplementConstructor(typeBuilder, ctorParameters, baseConstructor);

            var methodMetadatas = new List<MethodMetadata>(methods.Count + properties.Count);
            CreateMethods(methodMetadatas, methodId, typeBuilder, targetType, methods, false, interceptor.ContainsTarget);
            CreateProperties(methodMetadatas, methodId, typeBuilder, targetType, properties);
            
            return methodMetadatas.ToArray();
        }

        private void ImplementConstructor(TypeBuilder typeBuilder, Type[] parameterTypes, ConstructorInfo baseConstructor)
        {
            const MethodAttributes attributes = MethodAttributes.Public;

            var constructorBuilder = typeBuilder.DefineConstructor(
                attributes,
                baseConstructor.CallingConvention,
                parameterTypes
            );

            MethodFactory.EmitProxyConstructor(constructorBuilder.GetEmitter(), baseConstructor);
        }

        private static (List<MethodInfo>, List<PropertyInfo>) AddInterfaceImplementations(TypeBuilder typeBuilder, Type targetType)
        {
            if (!targetType.IsInterface)
            {
                throw new KimonoException($"Type must be an interface: {targetType}");
            }

            return AddMembersRecursive(typeBuilder, targetType);
        }

        private static (List<MethodInfo>, List<PropertyInfo>) AddMembersRecursive(TypeBuilder typeBuilder, Type interfaceType)
        {
            typeBuilder.AddInterfaceImplementation(interfaceType);

            var methods = GetBaseMethods();
            var properties = new List<PropertyInfo>();

            AddMethodsRecursive(methods, interfaceType);
            AddPropertiesRecursive(properties, interfaceType);

            return (methods, properties);
        }

        private static void AddMethodsRecursive(List<MethodInfo> methods, Type interfaceType)
        {
            methods.AddRange(GetMethods(interfaceType));

            foreach (Type type in interfaceType.GetInterfaces())
            {
                AddMethodsRecursive(methods, type);
            }
        }

        private static List<MethodInfo> GetMethods(Type interfaceType)
        {
            return new List<MethodInfo>(interfaceType.GetMethods()).FindAll(m => !m.IsSpecialName);
        }

        private static void AddPropertiesRecursive(List<PropertyInfo>properties, Type interfaceType)
        {
            properties.AddRange(interfaceType.GetProperties());

            foreach (Type type in interfaceType.GetInterfaces())
            {
                AddPropertiesRecursive(properties, type);
            }
        }

        private static List<MethodInfo> GetBaseMethods()
        {
            return new List<MethodInfo>(Types.Object.GetMethods()).FindAll(method => method.IsVirtual);
        }

        /// <summary>
        /// Defines generic parameters on a MethodBuilder by copying them from the original method.
        /// </summary>
        /// <remarks>
        /// When creating a proxy for a generic method like void Method&lt;T&gt;() where T : class,
        /// this method:
        /// 1. Creates new generic parameters on the MethodBuilder with the same names
        /// 2. Copies all generic parameter attributes (covariant, contravariant, etc.)
        /// 3. Copies all constraints (class, struct, new(), interface, base class)
        ///
        /// IMPORTANT: Returns the newly created generic parameter types, which MUST be used
        /// in subsequent IL generation (particularly in ldtoken instructions). Using the
        /// original method's generic parameter types will cause a BadImageException.
        ///
        /// Example: For void Log&lt;TState&gt;(TState state) in ILogger&lt;T&gt;:
        /// - Creates a new generic parameter "TState" on the proxy method
        /// - Copies any constraints from the original TState
        /// - Returns the new TState type for use in IL generation
        /// </remarks>
        /// <param name="metadata">Metadata about the method whose generic parameters to copy.</param>
        /// <param name="methodBuilder">The MethodBuilder to define generic parameters on.</param>
        /// <returns>Array of newly created generic parameter types that are valid in the MethodBuilder's context.</returns>
        private static Type[] DefineGenericParameters(MethodMetadata metadata, MethodBuilder methodBuilder)
        {
            var genericParameters = metadata.GenericParameters;

            // Create generic parameters with the same names as the original method
            var genericParameterBuilders = methodBuilder.DefineGenericParameters(genericParameters.Select(t => t.Name).ToArray());

            // Copy attributes and constraints from each original generic parameter
            for (int i = 0; i < genericParameterBuilders.Length; i++)
            {
                var baseGenericArgument = genericParameters[i];
                var genericParameterBuilder = genericParameterBuilders[i];

                // Copy generic parameter attributes (None, Covariant, Contravariant, ReferenceTypeConstraint, etc.)
                genericParameterBuilder.SetGenericParameterAttributes(baseGenericArgument.GenericParameterAttributes);

                // Copy constraints (interface constraints and base type constraints)
                foreach (var baseTypeConstraint in baseGenericArgument.GetGenericParameterConstraints())
                {
                    if (baseTypeConstraint.IsInterface)
                    {
                        genericParameterBuilder.SetInterfaceConstraints(baseTypeConstraint);
                    }
                    else
                    {
                        genericParameterBuilder.SetBaseTypeConstraint(baseTypeConstraint);
                    }
                }
            }

            // Return the newly created types - these are the types that must be used in IL generation
            return genericParameterBuilders.Cast<Type>().ToArray();
        }

        /// <summary>
        /// Creates proxy implementations for all methods in the interface.
        /// </summary>
        /// <remarks>
        /// For each method in the interface, this:
        /// 1. Creates a MethodBuilder with matching signature
        /// 2. For generic methods, defines generic parameters with DefineGenericParameters
        /// 3. Emits IL that forwards the call to the interceptor via HandleMethodCall
        /// 4. Optionally creates a delegate invoker for performance optimization
        ///
        /// Special handling:
        /// - IDisposable.Dispose gets a special implementation that calls HandleDisposeCall
        /// - Generic methods require DefineGenericParameters to create new generic types
        ///   that are valid in the MethodBuilder's IL context
        /// - Object methods (ToString, GetHashCode, etc.) are explicitly overridden
        ///
        /// The generic parameter types returned by DefineGenericParameters are passed to
        /// EmitProxyMethod to ensure IL generation uses the correct types (avoiding BadImageException).
        /// </remarks>
        /// <param name="metadatas">List to populate with method metadata.</param>
        /// <param name="methodId">Current method ID for tracking.</param>
        /// <param name="typeBuilder">TypeBuilder for the proxy type.</param>
        /// <param name="targetType">The interface type being proxied.</param>
        /// <param name="methods">Methods to implement.</param>
        /// <param name="areProperties">Whether these methods are property accessors.</param>
        /// <param name="generateInvoker">Whether to generate optimized invokers.</param>
        private void CreateMethods(List<MethodMetadata> metadatas, MethodId methodId, TypeBuilder typeBuilder, Type targetType, List<MethodInfo> methods, bool areProperties = false, bool generateInvoker = false)
        {
            // Special handling for IDisposable
            if (Types.Disposable.IsAssignableFrom(targetType) && methods.Remove(Methods.Dispose))
            {
                var disposeMethod = Methods.Dispose;
                var methodBuilder = typeBuilder.DefineMethod(disposeMethod.Name, disposeMethod.Attributes ^ MethodAttributes.Abstract);

                MethodFactory.EmitProxyDisposeMethod(Emitter.For(methodBuilder.GetILGenerator()));
            }

            // Create proxy implementation for each method
            methods.ForEach(methodInfo => {
                var metadata = MethodMetadata.FromMethodInfo(methodInfo, areProperties);
                metadatas.Insert(methodId, metadata);

                var parameterTypes = metadata.ParameterTypes;
                var methodBuilder = typeBuilder.DefineMethod(
                    methodInfo.Name,
                    methodInfo.IsAbstract
                        ? methodInfo.Attributes ^ MethodAttributes.Abstract
                        : methodInfo.Attributes,
                    methodInfo.ReturnType,
                    parameterTypes
                );

                // Explicit override for Object methods (ToString, GetHashCode, Equals)
                if (methodInfo.DeclaringType == Types.Object)
                {
                    typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
                }

                var emitter = methodBuilder.GetEmitter();

                // For generic methods, define generic parameters and capture the new types
                Type[]? genericParameterTypes = null;
                if (methodInfo.IsGenericMethod)
                {
                    // DefineGenericParameters creates new generic types on the MethodBuilder
                    // These MUST be used in IL generation instead of the original method's types
                    genericParameterTypes = DefineGenericParameters(metadata, methodBuilder);
                }

                // Emit IL that forwards the call to the interceptor
                MethodFactory.EmitProxyMethod(emitter, methodId, metadata, genericParameterTypes);

                // Optionally create an optimized delegate invoker for performance
                if (generateInvoker)
                {
                    metadata.UseInvoker(MethodFactory.CreateDelegateInvoker(metadata));
                }

                methodId++;
            });
        }

        private void CreateProperties(List<MethodMetadata> metadatas, MethodId methodId, TypeBuilder typeBuilder, Type targetType, List<PropertyInfo> properties, bool buildInvoker = false)
        {
            var metadataArray = new List<MethodInfo>(properties.Count);

            properties.ForEach(propertyInfo =>
            {
                if (propertyInfo.CanRead)
                {
                    metadataArray.Add(propertyInfo.GetGetMethod()!);
                }

                if (propertyInfo.CanWrite)
                {
                    metadataArray.Add(propertyInfo.GetSetMethod()!);
                }
            });

            CreateMethods(metadatas, methodId, typeBuilder, targetType, metadataArray, true, buildInvoker);
        }

        private static class Names
        {
            public const string DllName = "KimonoProxies.dll";
            public const string Namesapce = "KimonoProxies.{0}";
            public const string ModuleName = "KimonoProxies";
            public const string TypeName = "Proxy-{0}";
        }

        private static class Methods
        {
            public static readonly MethodInfo Dispose = Types.Disposable.GetMethod("Dispose")!;
        }
    }
}
