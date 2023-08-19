using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Proxies
{
	/// <summary>
	/// Interface IProxyMethodGenerator
	/// </summary>
	internal interface IProxyMethodGenerator
    {
		void Generate(IProxyContextBuilder context, IEnumerable<MethodInfo> methods);

        void EmitTypeInitializer(ILGenerator ilGenerator, ConstructorInfo baseConstructor);
    }
}