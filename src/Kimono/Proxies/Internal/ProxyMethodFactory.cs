using Kimono.Emit;
using Kimono.Proxies;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kimono.Internal
{
    internal sealed class ProxyMethodFactory : ProxyMemberFactory, IProxyMethodFactory
    {
        public ProxyMethodFactory(IDelegateFactory? delegateGenerator = null) : base(delegateGenerator)
        {
        }

        public void Create(IProxyContextBuilder context, IEnumerable<MethodInfo> methods)
        {
            CreateMethods(context, methods);
        }

        public void CreateMethod(IProxyContextBuilder context, MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters().Select(p =>
                new RuntimeParameter(p.Name!, type: p.ParameterType)
            ).ToList();

            var methodId = context.GetNextMethodId();
            var method = new ProxyMember(methodInfo);
            var methodBuilder = CreateMethod(context,
                methodInfo.IsGenericMethod
                    ? methodInfo.GetGenericMethodDefinition()
                    : methodInfo,
                parameters,
                methodId
            );

            var runtimeContext = new RuntimeContext(method, parameters);

            runtimeContext.UseMethodInvoker(
                DelegateFactory.CreateDelegateInvoker(methodInfo, parameters)
            );

            context.ProxyContext.Add(runtimeContext);
        }

        public void EmitConstructor(IEmitter emitter, ConstructorInfo baseConstructor)
        {
            DelegateFactory.EmitConstructor(emitter, baseConstructor);
        }

        private void CreateMethods(IProxyContextBuilder context, IEnumerable<MethodInfo> methods)
        {
            if (Constants.DisposableType.IsAssignableFrom(context.Interceptor.TargetType))
            {
                methods = methods.Where(m => m.DeclaringType != Constants.DisposableType);

                var disposeMethod = Constants.DisposeMethod;
                var methodBuilder = context.TypeBuilder.DefineMethod(disposeMethod.Name, disposeMethod.Attributes ^ MethodAttributes.Abstract);
                DelegateFactory.EmitDisposeDelegate(methodBuilder.GetEmitter(), Constants.DisposeMethod);
            }

            foreach (var methodInfo in methods)
            {
                CreateMethod(context, methodInfo);
            }
        }
    }
}
