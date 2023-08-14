using Kimono.Proxies;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Internal
{
	internal sealed class ProxyMethodGenerator : ProxyMemberGenerator, IProxyMethodGenerator
	{
		public void Generate(IProxyBuilderContext context, IEnumerable<MethodInfo> methods)
		{
			ImplementMethods(context, methods);
		}

		/// <summary>
		/// Implements the methods.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methods">The methods.</param>
		private static void ImplementMethods(IProxyBuilderContext context, IEnumerable<MethodInfo> methods)
		{
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

				context.ProxyContext.Add(new RuntimeContext(method,
					methodInfo.GetParameters().Select(p => new RuntimeParameter(p.Name!, type: p.ParameterType)))
				);
			}
		}
	}
}
