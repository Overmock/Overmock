using Kimono.Emit;
using System.Collections.Generic;
using System.Reflection;

namespace Kimono.Proxies
{
    /// <summary>
    /// Interface IProxyMethodGenerator
    /// </summary>
    internal interface IProxyMethodFactory
    {
        void Create(IProxyContextBuilder context, IEnumerable<MethodInfo> methods);

        void EmitConstructor(IEmitter emitter, ConstructorInfo baseConstructor);

        void CreateMethod(IProxyContextBuilder context, MethodInfo methodInfo);
    }
}