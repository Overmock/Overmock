using System.Reflection.Emit;
using System.Reflection;
using Kimono.Proxies;

namespace Kimono.Internal
{
	internal class ProxyMemberGenerator
    {
        private readonly IMethodDelegateGenerator _delegateDelegateGenerator;

        public ProxyMemberGenerator(IMethodDelegateGenerator? delegateDelegateGenerator = null)
        {
            _delegateDelegateGenerator = delegateDelegateGenerator ?? new DynamicMethodDelegateGenerator();
        }

        protected IMethodDelegateGenerator DelegateGenerator => _delegateDelegateGenerator;

        /// <summary>
        /// The kimono attribute constructor
        /// </summary>
        protected static readonly ConstructorInfo _kimonoAttributeConstructor = typeof(KimonoAttribute).GetConstructors().First();

		/// <summary>
		/// Creates the method.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodInfo">The method information.</param>
		/// <param name="methodId">The method identifier.</param>
		/// <returns>MethodBuilder.</returns>
		protected static MethodBuilder CreateMethod(IProxyContextBuilder context, MethodInfo methodInfo, int methodId)
		{
			var parameterTypes = methodInfo.GetParameters()
				.Select(p => p.ParameterType).ToArray();

			var methodBuilder = context.TypeBuilder.DefineMethod(
				methodInfo.Name,
				methodInfo.IsAbstract
					? methodInfo.Attributes ^ MethodAttributes.Abstract
					: methodInfo.Attributes,
				methodInfo.ReturnType,
				parameterTypes
			);

			if (methodInfo.DeclaringType == Constants.ObjectType)
			{
				context.TypeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
			}

			var iLGenerator = methodBuilder.GetILGenerator();
			var returnType = methodInfo.ReturnType;

			if (methodInfo.IsGenericMethod)
			{
				DefineGenericParameters(methodInfo, methodBuilder);
			}

			EmitMethodBody(context, iLGenerator, returnType, parameterTypes, methodId);

			return methodBuilder;
		}

		/// <summary>
		/// Emits the method body.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		/// <param name="returnType">Type of the return.</param>
		/// <param name="parameters">The parameters.</param>
		/// <param name="methodId">The method identifier.</param>
		protected static void EmitMethodBody(IProxyContextBuilder context, ILGenerator emitter, Type returnType, Type[] parameters, int methodId)
		{
			var returnIsNotVoid = returnType != Constants.VoidType;

			if (returnIsNotVoid)
			{
				emitter.DeclareLocal(returnType);
			}

			var returnLabel = emitter.DefineLabel();

			emitter.Emit(OpCodes.Nop);
			emitter.Emit(OpCodes.Ldarg_0);

			emitter.Emit(OpCodes.Ldc_I4, methodId);

			if (parameters.Length > 0)
			{
				emitter.Emit(OpCodes.Ldc_I4, parameters.Length);
				emitter.Emit(OpCodes.Newarr, Constants.ObjectType);

				for (int i = 0; i < parameters.Length - 1; i++)
				{
					emitter.Emit(OpCodes.Dup);
					emitter.Emit(OpCodes.Ldc_I4, i);
					emitter.Emit(OpCodes.Ldarg, i + 1);

					if (parameters[i].IsValueType)
					{
						emitter.Emit(OpCodes.Box, parameters[i]);
					}

					emitter.Emit(OpCodes.Stelem_Ref);
				}

				emitter.Emit(OpCodes.Dup);
				emitter.Emit(OpCodes.Ldc_I4, parameters.Length - 1);
				emitter.Emit(OpCodes.Ldarg, parameters.Length);
				emitter.Emit(OpCodes.Stelem_Ref);
			}
			else
			{
				emitter.EmitCall(OpCodes.Call, Constants.EmptyObjectArrayMethod, null);
			}

			emitter.EmitCall(OpCodes.Call, Constants.ProxyTypeHandleMethodCallMethod, null);

			if (returnIsNotVoid)
			{
				if (returnType.IsValueType)
				{
					emitter.Emit(OpCodes.Unbox_Any, returnType);
				}
				else
				{
					emitter.Emit(OpCodes.Castclass, returnType);

					emitter.Emit(OpCodes.Stloc_0);
					emitter.Emit(OpCodes.Br_S, returnLabel);

					emitter.MarkLabel(returnLabel);
					emitter.Emit(OpCodes.Ldloc_0);
				}
			}
			else
			{
				emitter.Emit(OpCodes.Pop);
			}

			emitter.Emit(OpCodes.Ret);
		}

		/// <summary>
		/// Defines the generic parameters.
		/// </summary>
		/// <param name="methodInfo">The method information.</param>
		/// <param name="methodBuilder">The method builder.</param>
		protected static void DefineGenericParameters(MethodInfo methodInfo, MethodBuilder methodBuilder)
		{
			var genericParameters = methodInfo.GetGenericArguments();
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
	}
}
