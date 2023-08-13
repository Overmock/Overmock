using System.Reflection;

namespace Kimono.Proxies
{
	/// <summary>
	/// Interface IProxyMethodGenerator
	/// </summary>
	internal interface IProxyMethodGenerator
    {
		void Generate(IProxyBuilderContext context, IEnumerable<MethodInfo> methods);
    }
}