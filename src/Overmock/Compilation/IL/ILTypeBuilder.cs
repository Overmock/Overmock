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

				CopyMethod(overmockedMethod, methodInfo);

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

		public static void CopyMethod(MethodBuilder methodBuilder, MethodInfo methodInfo)
		{
			var methodBody = methodInfo.GetMethodBody();
			var methodBodyIl = methodBody.GetILAsByteArray()!;
			var emitter = methodBuilder.GetILGenerator();
			var opCodes = GetOpCodes(methodBodyIl);

			foreach (var local in methodBody.LocalVariables)
			{
				emitter.DeclareLocal(local.LocalType);
			}

			for (var i = 1; i <= opCodes.Length; i++)
			{
				var opCode = opCodes[i];

				if (opCode.Code.OperandType == OperandType.InlineBrTarget)
				{
					emitter.Emit(opCode.Code, BitConverter.ToInt32(methodBodyIl, i));
					i += 4;
					continue;
				}
				
				if (opCode.Code.OperandType == OperandType.ShortInlineBrTarget)
				{
					emitter.Emit(opCode.Code, methodBodyIl[i]);
					++i;
					continue;
				}
				
				if (opCode.Code.OperandType == OperandType.InlineType)
				{
					Type tp = methodInfo.Module.ResolveType(BitConverter.ToInt32(methodBodyIl, i), methodInfo.DeclaringType.GetGenericArguments(), methodBuilder.GetGenericArguments());
					emitter.Emit(opCode.Code, tp);
					i += 4;
					continue;
				}

				if (opCode.Code.FlowControl == FlowControl.Call)
				{
					MethodInfo? mi = methodInfo.Module.ResolveMethod(BitConverter.ToInt32(methodBodyIl, i + 1)) as MethodInfo;
					if (mi == methodBuilder)
					{
						emitter.Emit(opCode.Code, methodBuilder);
					}
					else
					{
						emitter.Emit(opCode.Code, mi!);
					}
					
					i += 4;
					
					continue;
				}
				emitter.Emit(opCode.Code);
			}
		}

		public static OpCodeContainer[] GetOpCodes(byte[] data)
		{
			return data.Select(opCodeByte => new OpCodeContainer(opCodeByte)).ToArray();
		}

		public class OpCodeContainer
		{
			private static readonly OpCode[] _opCodeFields = typeof(OpCodes).GetFields().Select(f => (OpCode)f.GetValue(null)!).ToArray();
			private readonly OpCode _code;

			public OpCodeContainer(byte opCode)
			{
				try
				{
					_code = _opCodeFields.First(f => f.Value == opCode);
				}
				catch { }
			}

			public OpCode Code => _code;
		}
	}
}