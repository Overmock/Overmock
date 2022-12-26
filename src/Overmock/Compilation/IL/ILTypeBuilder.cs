using Overmock.Runtime;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace Overmock.Compilation.IL
{
	internal class ILTypeBuilder : ITypeBuilder
	{
		private Action<SetupArgs>? _argsProvider;

		public ILTypeBuilder(Action<SetupArgs>? argsProvider)
		{
			_argsProvider = argsProvider;
		}

		public T? BuildType<T>(IOvermock<T> target) where T : class
		{
			var assemblyName = new AssemblyName($"{target.TypeName}.dll");
			var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);

			var typeBuilder = GetTypeBuilder(target, assemblyBuilder);
			var contextField = typeBuilder.DefineField("___context___", typeof(OvermockContext), FieldAttributes.Private);
			var defaultConstructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { typeof(OvermockContext) });
			var overmockContext = new OvermockContext();
			var constructorBody = defaultConstructor.GetILGenerator();
			constructorBody.Emit(OpCodes.Ldarg_0);
			constructorBody.Emit(OpCodes.Ldarg_1);
			constructorBody.Emit(OpCodes.Stfld, contextField);
			
			foreach (var overmock in target.GetOvermockedMethods())
			{
				var overrides = overmock.GetOverrides();
				var methodInfo = overmock.Expression.Method;

				var parameters = methodInfo.GetParameters();
				var overmockedMethod = typeBuilder.DefineMethod(methodInfo.Name, MethodAttributes.Public, methodInfo.ReturnType, parameters.Select(p => p.ParameterType).ToArray());

				var emitter = overmockedMethod.GetILGenerator();

				emitter.Emit(OpCodes.Ldarg_0);
				emitter.Emit(OpCodes.Ldarg_1);

				// TODO: emit the following snippet.
				//{
				//	var handle = _context.Get((MethodInfo)MethodBase.GetCurrentMethod()!);
				//	var result = handle.Handle(name); // multiple parameters

				//	if (result.Result != null)
				//	{
				//		return (string)result.Result;
				//	}

				//	throw new OvermockException("oops, didn't handle this method call.");
				//}

				overmockContext.Add(methodInfo, new OverrideContext(methodInfo,
					overmock.GetOverrides(),
					parameters.Select(p => new OverrideParameter(p.Name!, type: p.ParameterType)))
				);
			}

			target.SetCompiledType(typeBuilder.CreateType()!);

			return (T)Activator.CreateInstance(target.GetCompiledType()!)!;
		}

		private static TypeBuilder GetTypeBuilder<T>(IOvermock<T> target, AssemblyBuilder assemblyBuilder) where T : class
		{
			var moduleBuilder = assemblyBuilder.DefineDynamicModule($"{target.TypeName}_module");

			if (target.Type.IsInterface)
			{
				var typeBuilder = moduleBuilder.DefineType(target.TypeName, TypeAttributes.Public);
				typeBuilder.AddInterfaceImplementation(typeof(T));
				return typeBuilder;
			}

			return moduleBuilder.DefineType(target.TypeName, TypeAttributes.Class | TypeAttributes.Public, typeof(T));
		}
	}
}