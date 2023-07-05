using Lokad.ILPack;
using Overmock.Mocking;
using Overmock.Runtime;
using Overmock.Runtime.Proxies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Overmock.Compilation.IL
{
    internal class ILTypeBuilder : ITypeBuilder
	{
        private static readonly Type MethodBaseType = typeof(MethodBase);
		private static readonly Type MethodInfoType = typeof(MethodInfo);
		private static readonly Type HandlerType = typeof(IRuntimeHandler);
		private static readonly Type ContextType = typeof(ProxyOverrideContext);

		private readonly Action<SetupArgs>? _argsProvider;
        private readonly IlAssemblyCompiler _compiler;

        public ILTypeBuilder(IlAssemblyCompiler compiler, Action<SetupArgs>? argsProvider)
        {
            _compiler = compiler;
            _argsProvider = argsProvider;
        }

        public T? BuildType<T>(IOvermock<T> target) where T : class
        {
            var overmockContextType = typeof(ProxyOverrideContext);
            var assemblyName = new AssemblyName($"Overmocked.Generated.dll");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);

            var overmockedMethods = target.GetOvermockedMethods().ToList();
            var typeBuilder = GetTypeBuilder(target, assemblyBuilder, overmockedMethods);
            var contextField = typeBuilder.DefineField("___context", overmockContextType, FieldAttributes.Private);

            DefineConstructor(typeBuilder);

            var overmockContext = DefineOvermockInitMethod<T>(typeBuilder, contextField);

            DefineMethods(target, typeBuilder, contextField, overmockedMethods.Select(m => m.Expression.Method).ToList());

            foreach (var overmock in overmockedMethods)
            {
                var methodInfo = overmock.Expression.Method;

                var methodBuilder = CreateMethod(methodInfo, typeBuilder, contextField);

                var methodId = Guid.NewGuid();
                methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(
                    typeof(OvermockAttribute).GetConstructors().First(),
                    new object[] { methodId.ToString() }
                ));

                overmockContext.Add(methodId, new RuntimeContext(methodInfo,
                    overmock.GetOverrides(),
                    methodInfo.GetParameters().Select(p => new RuntimeParameter(p.Name!, type: p.ParameterType)))
                );
            }
            
            var targetType = typeBuilder.CreateType()!;

            // Write the assembly to disc for testing
            var generator = new AssemblyGenerator();
            var fileName = assemblyBuilder.GetName().Name!;
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            File.WriteAllBytes(fileName, generator.GenerateAssemblyBytes(assemblyBuilder));
            // Write the assembly to disc for testing

            target.SetCompiledType(targetType);

            var instance = (T)Activator.CreateInstance(targetType)!;

            targetType.GetMethod("InitializeOvermockContext", new[] { overmockContextType })!
                .Invoke(instance, new object?[] { overmockContext });
			
            return instance;
        }

        private static void DefineConstructor(TypeBuilder typeBuilder)
        {
            var defaultConstructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
            defaultConstructor.GetILGenerator().Emit(OpCodes.Ret);
        }

        private static ProxyOverrideContext DefineOvermockInitMethod<T>(TypeBuilder typeBuilder, FieldBuilder contextField)
            where T : class
        {
            var overmockContext = new ProxyOverrideContext();

            var initContextMethod = typeBuilder.DefineMethod("InitializeOvermockContext",
                MethodAttributes.Public, CallingConventions.HasThis, typeof(void),
                new[] { contextField.FieldType }
            );
            var initContextMethodBody = initContextMethod.GetILGenerator();
            initContextMethodBody.Emit(OpCodes.Ldarg_0);
            initContextMethodBody.Emit(OpCodes.Ldarg_1);
            initContextMethodBody.Emit(OpCodes.Stfld, contextField);
            initContextMethodBody.Emit(OpCodes.Ret);
            return overmockContext;
        }

        private static TypeBuilder GetTypeBuilder<T>(IOvermock<T> target, AssemblyBuilder assemblyBuilder, IReadOnlyList<IMethodCall> overmockedMethods) where T : class
		{
			static string getFullTypeName(string typeName)
			{
				return $"OvermockGenerated.{typeName}";
			}

			var moduleBuilder = assemblyBuilder.DefineDynamicModule(
				GetDynamicTypeName(target.Type, suffix: "_Module"));

			if (target.Type.IsInterface)
			{
				var typeBuilder = moduleBuilder.DefineType(getFullTypeName(target.TypeName), TypeAttributes.Class | TypeAttributes.Public);
				typeBuilder.AddInterfaceImplementation(typeof(T));
				return typeBuilder;
			}

			return moduleBuilder.DefineType(getFullTypeName(target.TypeName), TypeAttributes.Class | TypeAttributes.Public, typeof(T));
		}

		protected static void DefineMethods(IOvermock target, TypeBuilder dynamicType, FieldBuilder contextField, IReadOnlyList<MethodInfo> excludedMethods)
		{
			DefineMethods(target.Type, dynamicType, contextField, excludedMethods);
		}

		protected static void DefineMethods(Type target, TypeBuilder dynamicType, FieldBuilder contextField, IReadOnlyList<MethodInfo> excludedMethods)
		{
			var methods = target.GetMethods()
				.Where(m => !m.IsSpecialName && !excludedMethods.Contains(m));

			foreach (MethodInfo methodInfo in methods)
			{
				CreateMethod(methodInfo, dynamicType, contextField);
			}
		}

		protected static MethodBuilder CreateMethod(MethodInfo methodInfo, TypeBuilder dynamicType, FieldBuilder contextField)
		{
			object methodToCopy(IRuntimeHandler handler, params object[] parameters)
			{
				var result = handler.Handle();
				return result.Result;
			}

			var parameterTypes = methodInfo.GetParameters()
				.Select(p => p.ParameterType).ToArray();

			var methodBuilder = dynamicType.DefineMethod(
				methodInfo.Name,
				methodInfo.Attributes ^ (MethodAttributes.Abstract | MethodAttributes.Virtual),
				methodInfo.ReturnType,
				parameterTypes
			);

			CopyMethod(methodBuilder, contextField, parameterTypes);

			//dynamicType.DefineMethodOverride(methodBuilder, typeof(OvermockMethodTemplate).GetMethod("TestMethod", BindingFlags.Public | BindingFlags.Instance)!);

			//var dynamicMethod = new DynamicMethod(methodInfo.Name,
			//	methodInfo.ReturnType,
			//	new[] { HandlerType, typeof(object[]) },
			//	dynamicType.Module);

			//var dynamicIL = dynamicMethod.GetDynamicILInfo();

			//var delegateType = (Func<IOverrideHandler, object[], object>)methodToCopy;
			//var delegateBody = delegateType.Method.GetMethodBody();

			//dynamicIL.SetCode(delegateBody.GetILAsByteArray(), delegateBody.MaxStackSize);

			//var overrideMethod = dynamicMethod.CreateDelegate(delegateType.GetType());
			//var overrideMethodHandle = delegateType.Method.MethodHandle;
			////

			////dynamicType.DefineMethodOverride(methodBuilder, dynamicMethod);
			//RuntimeHelpers.PrepareMethod(methodInfo.MethodHandle);
			//RuntimeHelpers.PrepareMethod(overrideMethod.Method.MethodHandle);

			//Marshal.WriteIntPtr(methodBuilder.MethodHandle.Value, delegateType.Method.MethodHandle.Value);

			return methodBuilder;
		}

		protected static void CopyMethod(MethodBuilder methodBuilder, FieldBuilder contextField, Type[] parameters)
		{
			var emitter = methodBuilder.GetILGenerator();

            if (methodBuilder.ReturnType != typeof(void))
            {
                emitter.DeclareLocal(methodBuilder.ReturnType);
            }

            emitter.DeclareLocal(HandlerType);
			emitter.DeclareLocal(typeof(RuntimeHandlerResult));
			emitter.DeclareLocal(typeof(object));

			var returnLabel = emitter.DefineLabel();

			emitter.Emit(OpCodes.Nop);
			emitter.Emit(OpCodes.Ldarg_0);

			//emitter_Emit(OpCodes.ldfld class Overmock.Runtime.OvermockContext Overmock.OvermockMethodTemplate::___context___
			emitter.Emit(OpCodes.Ldfld, contextField);

			//emitter.Emit(OpCodes.call class [System.Runtime]System.Reflection.MethodBase[System.Runtime] System.Reflection.MethodBase::GetCurrentMethod()
			emitter.EmitCall(OpCodes.Call,
				MethodBaseType.GetMethod("GetCurrentMethod", BindingFlags.Static | BindingFlags.Public)!,
				Type.EmptyTypes
			);

			emitter.Emit(OpCodes.Castclass, MethodInfoType);

			//emitter.Emit(OpCodes.callvirt instance class Overmock.Runtime.IOverrideHandler Overmock.Runtime.OvermockContext::Get(class [System.Runtime] System.Reflection.MethodInfo)
			emitter.EmitCall(OpCodes.Callvirt,
				ContextType.GetMethod("Get", BindingFlags.Instance | BindingFlags.Public)!,
				new[] { MethodInfoType }
			);

			emitter.Emit(OpCodes.Stloc_0);
			emitter.Emit(OpCodes.Ldloc_0);
			emitter.Emit(OpCodes.Ldc_I4_1);
			emitter.Emit(OpCodes.Newarr, typeof(object));
			emitter.Emit(OpCodes.Dup);
			emitter.Emit(OpCodes.Ldc_I4_0);
			emitter.Emit(OpCodes.Ldarg_1);
			emitter.Emit(OpCodes.Stelem_Ref);

			//emitter.Emit(OpCodes.callvirt instance class Overmock.Runtime.OverrideHandlerResult Overmock.Runtime.IOverrideHandler::Handle(object[])
			emitter.EmitCall(OpCodes.Callvirt, HandlerType.GetMethod("Handle",
				BindingFlags.Instance | BindingFlags.Public)!,
				new[] { typeof(object[]) }
			);

			emitter.Emit(OpCodes.Stloc_1);
			emitter.Emit(OpCodes.Ldloc_1);
			emitter.EmitCall(OpCodes.Callvirt,
				typeof(RuntimeHandlerResult).GetProperty("Result",
					BindingFlags.Public | BindingFlags.Instance)!
				.GetGetMethod()!,
				Type.EmptyTypes
			);

			emitter.Emit(OpCodes.Castclass, methodBuilder.ReturnType);

			emitter.Emit(OpCodes.Stloc_2);
			emitter.Emit(OpCodes.Br_S, returnLabel);

			emitter.MarkLabel(returnLabel);
			emitter.Emit(OpCodes.Ldloc_2);
			emitter.Emit(OpCodes.Ret);
		}

		protected static void DefineProperties(IOvermock target, TypeBuilder dynamicType)
		{
			DefineProperties(target.Type, dynamicType);
		}

		protected static void DefineProperties(Type target, TypeBuilder dynamicType)
		{
			foreach (PropertyInfo property in target.GetProperties())
			{
				CreateProperty(property, dynamicType);
			}
		}

		protected static TypeBuilder CreateProperty(PropertyInfo propertyInfo, TypeBuilder typeBuilder)
		{
			PropertyBuilder property = typeBuilder.DefineProperty(propertyInfo.Name, PropertyAttributes.HasDefault, CallingConventions.HasThis, propertyInfo.PropertyType, new Type[1]
			{
		propertyInfo.PropertyType
			});
			FieldBuilder field = typeBuilder.DefineField("_" + propertyInfo.Name.ToLower(), propertyInfo.PropertyType, FieldAttributes.Private);
			if (propertyInfo.CanRead)
				CreateGetMethod(typeBuilder, property, propertyInfo.GetGetMethod(), field, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName);
			if (propertyInfo.CanWrite)
				CreateSetMethod(typeBuilder, property, propertyInfo.GetSetMethod(), field, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName);
			return typeBuilder;
		}

		protected static void CreateGetMethod(
			TypeBuilder baseType,
			PropertyBuilder property,
			MethodInfo method,
			FieldInfo field,
			MethodAttributes getAttrs)
		{
			MethodBuilder mdBuilder = baseType.DefineMethod(method.Name, getAttrs, method.ReturnType, Type.EmptyTypes);
			ILGenerator ilGenerator = mdBuilder.GetILGenerator();
			ilGenerator.Emit(OpCodes.Ldarg_0);
			ilGenerator.Emit(OpCodes.Ldfld, field);
			ilGenerator.Emit(OpCodes.Ret);
			property.SetGetMethod(mdBuilder);
		}

		protected static void CreateSetMethod(
			TypeBuilder baseType,
			PropertyBuilder property,
			MethodInfo method,
			FieldInfo field,
			MethodAttributes setAttrs)
		{
			MethodBuilder mdBuilder = baseType.DefineMethod(method.Name, setAttrs, method.ReturnType, new Type[1]
			{
		field.FieldType
			});
			ILGenerator ilGenerator = mdBuilder.GetILGenerator();
			ilGenerator.Emit(OpCodes.Ldarg_0);
			ilGenerator.Emit(OpCodes.Ldarg_1);
			ilGenerator.Emit(OpCodes.Stfld, field);
			ilGenerator.Emit(OpCodes.Ret);
			property.SetSetMethod(mdBuilder);
		}

		protected static string GetDynamicTypeName(Type baseType, string prefix = "", string suffix = "") =>
			$"{prefix}{baseType.FullName}{suffix}";

		//public static void CopyMethod2(MethodBuilder methodBuilder, MethodInfo methodInfo)
		//{
		//	//emitter.Emit(OpCodes.Nop);
		//	//emitter.Emit(OpCodes.Ldarg_0);
		//	//emitter.Emit(OpCodes.Ldfld, contextField);

		//	//emitter.EmitWriteLine("I got to here");

		//	//emitter.EmitCall(OpCodes.Call,
		//	//	MethodBaseType.GetMethod("GetCurrentMethod", BindingFlags.Static | BindingFlags.Public)!,
		//	//	Type.EmptyTypes
		//	//);

		//	//emitter.Emit(OpCodes.Castclass, MethodInfoType);

		//	//emitter.EmitCall(OpCodes.Callvirt,
		//	//	ContextType.GetMethod("Get", BindingFlags.Instance | BindingFlags.Public)!,
		//	//	new[] { MethodInfoType }
		//	//);

		//	//emitter.Emit(OpCodes.Stloc_0);
		//	//emitter.Emit(OpCodes.Ldloc_0);
		//	//emitter.Emit(OpCodes.Ldc_I4_1);

		//	////TODO: Loop through all the parameters and set the params array parameters
		//	//emitter.Emit(OpCodes.Newarr, typeof(object));
		//	//emitter.Emit(OpCodes.Dup);
		//	//emitter.Emit(OpCodes.Ldc_I4_0);
		//	//emitter.Emit(OpCodes.Ldarg_1);
		//	//emitter.Emit(OpCodes.Stelem_Ref);
		//	//emitter.EmitCall(OpCodes.Callvirt, HandlerType.GetMethod("Handle",
		//	//	BindingFlags.Instance | BindingFlags.Public)!,
		//	//	new[] { typeof(object[]) }
		//	//);
		//	//emitter.Emit(OpCodes.Stloc_1);
		//	//emitter.Emit(OpCodes.Ldloc_1);
		//	//emitter.Emit(OpCodes.Castclass, methodBuilder.ReturnType);
		//	//emitter.Emit(OpCodes.Stloc_2);
		//	//emitter.Emit(OpCodes.Ldloc_2);
		//	//emitter.Emit(OpCodes.Ret);

		//	var methodBody = methodInfo.GetMethodBody();
		//	var methodBodyIl = methodBody.GetILAsByteArray();
		//	var emitter = methodBuilder.GetILGenerator();
		//	var opCodes = GetOpCodes(methodBodyIl);

		//	foreach (var local in methodBody.LocalVariables)
		//	{
		//		emitter.DeclareLocal(local.LocalType);
		//	}

		//	for (var i = 1; i <= opCodes.Length; i++)
		//	{
		//		var opCode = opCodes[i];

		//		if (opCode.Code.OperandType == OperandType.InlineBrTarget)
		//		{
		//			emitter.Emit(opCode.Code, BitConverter.ToInt32(methodBodyIl, i));
		//			i += 4;
		//			continue;
		//		}

		//		if (opCode.Code.OperandType == OperandType.ShortInlineBrTarget)
		//		{
		//			emitter.Emit(opCode.Code, methodBodyIl[i++]);
		//			continue;
		//		}

		//		if (opCode.Code.OperandType == OperandType.InlineType)
		//		{
		//			Type tp = methodInfo.Module.ResolveType(BitConverter.ToInt32(methodBodyIl, i), methodInfo.DeclaringType.GetGenericArguments(), methodBuilder.GetGenericArguments());
		//			emitter.Emit(opCode.Code, tp);
		//			i += 4;
		//			continue;
		//		}

		//		if (opCode.Code.FlowControl == FlowControl.Call)
		//		{
		//			MethodInfo? mi = methodInfo.Module.ResolveMethod(BitConverter.ToInt32(methodBodyIl, i + 1)) as MethodInfo;

		//			if (mi == null) { continue; }

		//			if (mi == methodBuilder)
		//			{
		//				emitter.Emit(opCode.Code, methodBuilder);
		//			}
		//			else
		//			{
		//				emitter.Emit(opCode.Code, mi!);
		//			}

		//			i += 4;

		//			continue;
		//		}
		//		emitter.Emit(opCode.Code);
		//	}
		//}

		//public static OpCodeContainer[] GetOpCodes(byte[] data)
		//{
		//	return data.Select(opCodeByte => new OpCodeContainer(opCodeByte)).ToArray();
		//}

		//public class OpCodeContainer
		//{
		//	private static readonly OpCode[] _opCodeFields = typeof(OpCodes).GetFields().Select(f => (OpCode)f.GetValue(null)!).ToArray();
		//	private readonly OpCode _code;

		//	public OpCodeContainer(byte opCode)
		//	{
		//		try
		//		{
		//			_code = _opCodeFields.First(f => f.Value == opCode);
		//		}
		//		catch { }
		//	}

		//	public OpCode Code => _code;
		//}
	}
}