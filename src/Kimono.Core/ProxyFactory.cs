using Kimono.Core.Internal;
using Kimono.Emit;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Core
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
                AssemblyBuilderAccess.Run);

            Module = Assembly.DefineDynamicModule(Names.ModuleName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delegateFactory"></param>
        public ProxyFactory(IDelegateFactory delegateFactory, IProxyCache cache)
        {
            MethodFactory = delegateFactory;
            Cache = cache;
        }

        private static AssemblyBuilder Assembly { get; }

        private static ModuleBuilder Module { get; }

        public IDelegateFactory MethodFactory { get; }

        public IProxyCache Cache { get; }

        public static IProxyFactory Create(IDelegateFactory? factory = null, IProxyCache? cache = null)
        {
            return new ProxyFactory(
                factory ?? DelegateFactory.Current,
                cache ?? ProxyCache.Instance
            );
        }

        public T CreateInterfaceProxy<T>(IInterceptor<T> interceptor) where T : class
        {
            const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

            var targetType = typeof(T);
            var proxyBaseType = Types.ProxyBase.MakeGenericType(targetType);

            var typeBuilder = Module.DefineType(
                string.Format(Names.TypeName, typeof(T).Name),
                TypeAttributes.Public | TypeAttributes.Sealed,
                proxyBaseType);

            //ctor
            var ctorParameters = Types.ProxyBaseCtorParameterTypes<T>();
            var baseConstructor = proxyBaseType.GetConstructor(
                bindingFlags, null,
                ctorParameters,
                null
            )!;

            ImplementConstructor(typeBuilder, ctorParameters, baseConstructor);

            (var methods, var properties) = AddInterfaceImplementations(typeBuilder, targetType);

            var context = new ProxyContext();

            var methodId = MethodId.Create();
            CreateMethods(context, methodId, typeBuilder, targetType, methods);

            //properties
            CreateProperties(context, typeBuilder, targetType, properties);

            var proxyType = typeBuilder.CreateType();
            var proxyConstructor = proxyType.GetConstructor(ctorParameters)!;
            var generator =  Cache.SetGenerator(targetType,
                new ProxyGenerator<T>(context, MethodFactory.CreateProxyConstructorDelegate<T>(proxyType, targetType, proxyConstructor)
            ));

            return generator.GenerateProxy(interceptor);
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
            var properties = new List<PropertyInfo>(interfaceType.GetProperties());

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
        /// Defines the generic parameters.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="methodBuilder">The method builder.</param>
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

        private void CreateMethods(ProxyContext context, MethodId methodId, TypeBuilder typeBuilder, Type targetType, List<MethodInfo> methods)
        {
            if (Types.Disposable.IsAssignableFrom(targetType))
            {
                methods.Remove(Methods.Dispose);

                var disposeMethod = Methods.Dispose;
                var methodBuilder = typeBuilder.DefineMethod(disposeMethod.Name, disposeMethod.Attributes ^ MethodAttributes.Abstract);
                MethodFactory.EmitProxyDisposeMethod(Emitter.For(methodBuilder.GetILGenerator()), Methods.Dispose);
            }

            methods.ForEach(methodInfo => {
                var metadata = MethodMetadata.FromMethod(methodInfo);
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
                var returnType = methodInfo.ReturnType;

                if (methodInfo.IsGenericMethod)
                {
                    DefineGenericParameters(metadata, methodBuilder);
                }

                MethodFactory.EmitProxyMethod(emitter, methodId, metadata);
            });
        }

        private void CreateProperties(ProxyContext context, TypeBuilder typeBuilder, Type targetType, List<PropertyInfo> properties)
        {
        }
        
        private static class Names
        {
            public const string DllName = "KimonoProxies.{0}.dll";
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
