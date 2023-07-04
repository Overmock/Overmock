using System.Reflection;
using System.Reflection.Emit;

namespace Overmock.Runtime.Marshalling
{
    /// <summary>
    /// 
    /// </summary>
    public class InterfaceProxyMarshaller : ProxyMarshaller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="argsProvider"></param>
        internal InterfaceProxyMarshaller(IOvermock target, Action<SetupArgs>? argsProvider) : base(target, argsProvider)
        {
            DynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(
                GetAssemblyDllName(Target), AssemblyBuilderAccess.RunAndCollect
            );
            Name = GetAssemblyName(Target);
            DynamicModule = DynamicAssembly.DefineDynamicModule(Name);
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        private AssemblyBuilder DynamicAssembly { get; }

        /// <summary>
        /// 
        /// </summary>
        public ModuleBuilder DynamicModule { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override T Marshal<T>() where T : class
        {
            return CreateInterfaceProxy<T>(Target, DynamicModule);
        }

        private static T CreateInterfaceProxy<T>(IOvermock target, ModuleBuilder moduleBuilder) where T : class
        {
            const TypeAttributes attributes = TypeAttributes.Class | TypeAttributes.Public;
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var proxyType = RuntimeConstants.ProxyType.MakeGenericType(typeof(T));
            var proxyCtor = proxyType.GetConstructor(bindingFlags, new Type[]{ target.GetType() });
            var typeBuilder = moduleBuilder.DefineType(target.TypeName, attributes, proxyType);
            
            var context = new MarshallerContext(target, typeBuilder);

            ImplementConstructor(context, proxyCtor!);

            var overmockedMethods = target.GetOvermockedMethods().ToArray();
            var overrides = overmockedMethods.Select(m => m.Expression.Method);

            var methods = AddInterfaceImplementations(context)
                .Concat(GetBaseMethods(context))
                .Where(m => overrides.Contains(m))
                .Distinct();

            ImplementMethods(context, overmockedMethods, methods);

            return (T)Activator.CreateInstance(typeBuilder.CreateType(), target)!;
        }

        private static void ImplementMethods(MarshallerContext context, IEnumerable<IMethodCall> overmocks, IEnumerable<MethodInfo> methods)
        {
            foreach (var methodInfo in methods)
            {
                //var method = overmocks.Where(m => m.e)
                //if (Equals(methodInfo, ))
                //{
                    
                //}
            }
        }

        private static IEnumerable<MethodInfo> AddInterfaceImplementations(MarshallerContext context)
        {
            if (!context.Target.IsInterface())
            {
                throw new OvermockException(Ex.Message.NotAnInterfaceType(context.Target));
            }

            return AddInterfacesRecursive(context, context.Target.Type);
        }

        private static IEnumerable<MethodInfo> AddInterfacesRecursive(MarshallerContext context, Type interfaceType)
        {
            context.TypeBuilder.AddInterfaceImplementation(interfaceType);

            context.AddInterfaces(interfaceType);

            var methods = interfaceType.GetMethods().AsEnumerable();

            foreach (Type type in interfaceType.GetInterfaces())
            {
                methods = AddInterfacesRecursive(context, type).Concat(methods);
            }

            return methods.Distinct();
        }

        private static void ImplementConstructor(MarshallerContext context, ConstructorInfo baseConstructor)
        {
            const MethodAttributes methodAttributes = MethodAttributes.Public;

            var callingConvention = baseConstructor.CallingConvention;
            var parameterTypes = (from parameter in baseConstructor.GetParameters()
                    select parameter.ParameterType)
                .ToList();

            var constructorBuilder = context.TypeBuilder.DefineConstructor(methodAttributes, callingConvention, parameterTypes.ToArray());

            var ilGenerator = constructorBuilder.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);

            for (int i = 0; i < parameterTypes.Count; i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg, i + 1);
            }

            ilGenerator.Emit(OpCodes.Call, baseConstructor);
            ilGenerator.Emit(OpCodes.Ret);
        }

        private static IEnumerable<MethodInfo> GetBaseMethods(MarshallerContext context)
        {
            if (context.Target.IsDelegate())
            {
                return new[] { context.Target.Type.GetMethod(RuntimeConstants.InvokeMethodName)! };
            }

            return typeof(object).GetMethods().Where(method => method.IsVirtual);
        }

        private class MarshallerContext
        {
            public MarshallerContext(IOvermock target, TypeBuilder typeBuilder)
            {
                Target = target;
                TypeBuilder = typeBuilder;
                Interfaces = new List<Type>();
            }

            public IOvermock Target { get; }

            public TypeBuilder TypeBuilder { get; }

            private List<Type> Interfaces { get; set; }

            public void AddInterfaces(params Type[] interfaceTypes)
            {
                var interfaces = Interfaces.ToArray();

                Interfaces = interfaces.Union(interfaceTypes).Distinct().ToList();
            }
        }
    }
}