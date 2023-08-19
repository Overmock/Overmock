using Kimono.Emit;
using Kimono.Proxies;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Internal
{
    internal sealed class ProxyMethodGenerator : ProxyMemberGenerator, IProxyMethodGenerator
	{
        public ProxyMethodGenerator(IMethodDelegateGenerator? delegateGenerator = null) : base(delegateGenerator)
        {
        }

        public void Generate(IProxyContextBuilder context, IEnumerable<MethodInfo> methods)
		{
			ImplementMethods(context, methods);
		}

        public void EmitTypeInitializer(ILGenerator ilGenerator, ConstructorInfo baseConstructor)
        {
            DelegateGenerator.EmitTypeInitializer(Emitter.For(ilGenerator), baseConstructor);
        }

        public void GenerateProxyDelegate(RuntimeContext context, MethodInfo method)
        {
            context.UseMethodInvoker(DelegateGenerator.GenerateDelegateInvoker(context, method));
        }

		private void ImplementMethods(IProxyContextBuilder context, IEnumerable<MethodInfo> methods)
		{
			if (Constants.DisposableType.IsAssignableFrom(context.Interceptor.TargetType))
			{
				methods = methods.Where(m => m.DeclaringType != Constants.DisposableType);
                DelegateGenerator.EmitDisposeInterceptor(context, Constants.DisposeMethod);
			}

			foreach (var methodInfo in methods)
			{
				var methodId = context.GetNextMethodId();
				var method = new ProxyMember(methodInfo);
				var methodBuilder = CreateMethod(context,
					methodInfo.IsGenericMethod
						? methodInfo.GetGenericMethodDefinition()
						: methodInfo,
					methodId
				);

				methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(
					_kimonoAttributeConstructor,
					new object[] { methodId }
				));

                var runtimeContext = new RuntimeContext(method,
                    methodInfo.GetParameters().Select(p => 
                        new RuntimeParameter(p.Name!, type: p.ParameterType)));
				
				context.ProxyContext.Add(runtimeContext);

                GenerateProxyDelegate(runtimeContext, methodInfo);
            }
		}
	}
}
