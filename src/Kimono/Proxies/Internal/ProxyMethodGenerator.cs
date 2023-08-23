using Kimono.Emit;
using Kimono.Proxies;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Internal
{
    internal sealed class ProxyMethodGenerator : ProxyMemberGenerator, IProxyMethodFactory
	{
        public ProxyMethodGenerator(IMethodDelegateGenerator? delegateGenerator = null) : base(delegateGenerator)
        {
        }

        public void Create(IProxyContextBuilder context, IEnumerable<MethodInfo> methods)
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

                var runtimeContext = new RuntimeContext(method,
                    methodInfo.GetParameters().Select(p => 
                        new RuntimeParameter(p.Name!, type: p.ParameterType)));
				
				context.ProxyContext.Add(runtimeContext);

                GenerateProxyDelegate(runtimeContext, methodInfo);
            }
		}
	}
}
