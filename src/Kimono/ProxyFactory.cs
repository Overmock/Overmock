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

        /// <inheritdoc />
        public IProxyGenerator<T> CreateProxyGenerator<T>(IInterceptor<T> interceptor) where T : class
        {
            const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            
            var targetType = typeof(T);
            var proxyBaseType = Types.ProxyBaseNonGeneric;
            var methodId = MethodId.Create();
            var typeBuilder = Module.DefineType(
                string.Format(CultureInfo.CurrentCulture, Names.TypeName, targetType.Name),
                TypeAttributes.Public | TypeAttributes.Sealed,
                proxyBaseType);

            var ctorParameters = Types.ProxyBaseCtorParameterTypes;
            var baseConstructor = proxyBaseType.GetConstructor(
                bindingFlags, null,
                ctorParameters,
                null
            )!;

            var metadatas = BuildTypeMetadata(
                interceptor,
                targetType,
                methodId,
                typeBuilder,
                ctorParameters,
                baseConstructor
            );

            var proxyType = typeBuilder.CreateType() ?? throw new KimonoException($"Failed to create proxy type for: {targetType}");

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

        private static void DefineGenericParameters(MethodMetadata metadata, MethodBuilder methodBuilder)
        {
            var genericParameters = metadata.GenericParameters;
            var genericParameterBuilders = methodBuilder.DefineGenericParameters(genericParameters.Select(t => t.Name).ToArray());

            for (int i = 0; i < genericParameterBuilders.Length; i++)
            {
                var baseGenericArgument = genericParameters[i];
                var genericParameterBuilder = genericParameterBuilders[i];

                genericParameterBuilder.SetGenericParameterAttributes(baseGenericArgument.GenericParameterAttributes);

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
        }

        private void CreateMethods(List<MethodMetadata> metadatas, MethodId methodId, TypeBuilder typeBuilder, Type targetType, List<MethodInfo> methods, bool areProperties = false, bool generateInvoker = false)
        {
            if (Types.Disposable.IsAssignableFrom(targetType) && methods.Remove(Methods.Dispose))
            {
                var disposeMethod = Methods.Dispose;
                var methodBuilder = typeBuilder.DefineMethod(disposeMethod.Name, disposeMethod.Attributes ^ MethodAttributes.Abstract);

                MethodFactory.EmitProxyDisposeMethod(Emitter.For(methodBuilder.GetILGenerator()));
            }

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

                if (methodInfo.DeclaringType == Types.Object)
                {
                    typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
                }

                var emitter = methodBuilder.GetEmitter();

                if (methodInfo.IsGenericMethod)
                {
                    DefineGenericParameters(metadata, methodBuilder);
                }

                MethodFactory.EmitProxyMethod(emitter, methodId, metadata);

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
