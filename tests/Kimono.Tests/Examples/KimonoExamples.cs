using Kimono.Tests.Proxies;

namespace Kimono.Tests.Examples
{
    [TestClass]
    public class KimonoExamples
    {
        private readonly IProxyFactory _factory = ProxyFactory.Create();

        [TestMethod]
        public void NoTargetWithCallbackInterceptorExample()
        {
            var overmock = new TestCallbackInterceptor<IRepository>(callback: context => {
                if (context.Method.Name == "Baz")
                {
                    context.ReturnValue = context.GetParameter<Model>("model");
                }

                if (context.Method.Name == nameof(IRepository.Save))
                {
                    context.Invoke();
                }
            });
            
            var subject = _factory.CreateInterfaceProxy(overmock);

            subject.Save(new Model { Id = 20 });
        }
        [TestMethod]
        public void TargetWithCallbackInterceptorExample()
        {
            var overmock = new TestCallbackInterceptor<IRepository>(new Repository(), context => {
                context.Invoke();

                if (context.Method.Name == nameof(IRepository.Save))
                {
                    if (context.ReturnValue is false)
                    {
                        //Log failure
                    }
                }
            });

            var subject = _factory.CreateInterfaceProxy(overmock);

            subject.Save(new Model { Id = 20 });
        }
        [TestMethod]
        public void NoTargetWithHandlersInterceptorExample()
        {
            var overmock = new TestCallbackInterceptor<IRepository>(callback: i => { });

            var subject = _factory.CreateInterfaceProxy(overmock);

            subject.Save(new Model { Id = 20 });
        }
        [TestMethod]
        public void TargetWithHandlersInterceptorExample()
        {
            var overmock = new TestCallbackInterceptor<IRepository>(new Repository(), c => { });

            var subject = _factory.CreateInterfaceProxy(overmock);

            subject.Save(new Model { Id = 20 });
        }
        //[TestMethod]
        //public void TargetWithInvocationChainInterceptorExample()
        //{
        //    var interceptor = Intercept.WithInovcationChain<IRepository>(builder => {
        //        builder.Add((next, context) => {
        //            if (context.MemberName == nameof(IRepository.Save))
        //            {
        //                context.ReturnValue = true;
        //            }

        //            // Call next regardless of condition
        //            next(context);
        //        });
        //    });

        //    interceptor.Save(new Model { Id = 20 });
        //}
        //private class BazReturnInvocationHandler : IInvocationHandler
        //{
        //    public void Handle(IInvocationContext context)
        //    {
        //        if (context.MemberName == "Baz")
        //        {
        //            context.ReturnValue = context.GetParameter<Model>("model");
        //        }

        //        if (context.MemberName == nameof(IRepository.Save))
        //        {
        //            if (context.ReturnValue is false)
        //            {
        //                //Log failure
        //                return;
        //            }
        //        }
        //    }
        //}
    }
}
