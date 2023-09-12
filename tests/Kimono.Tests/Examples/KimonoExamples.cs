using Kimono.Tests.Proxies;

namespace Kimono.Tests.Examples
{
    [TestClass]
    public class KimonoExamples
    {
        [TestMethod]
        public void NoTargetWithCallbackInterceptorExample()
        {
            var interceptor = Intercept.WithCallback<IRepository>(context => {
                if (context.MemberName == "Baz")
                {
                    context.ReturnValue = context.GetParameter<Model>("model");
                }

                if (context.MemberName == nameof(IRepository.Save))
                {
                    context.Invoke();
                }
            });

            interceptor.Save(new Model { Id = 20 });
        }
        [TestMethod]
        public void TargetWithCallbackInterceptorExample()
        {
            var interceptor = Intercept.WithCallback<IRepository, Repository>(new Repository(), context => {
                context.Invoke();

                if (context.MemberName == nameof(IRepository.Save))
                {
                    if (context.ReturnValue is false)
                    {
                        //Log failure
                    }
                }
            });

            interceptor.Save(new Model { Id = 20 });
        }
        [TestMethod]
        public void NoTargetWithHandlersInterceptorExample()
        {
            var interceptor = Intercept.WithHandlers<IRepository>(new BazReturnInvocationHandler());

            interceptor.Save(new Model { Id = 20 });
        }
        [TestMethod]
        public void TargetWithHandlersInterceptorExample()
        {
            var interceptor = Intercept.WithHandlers<IRepository, Repository>(new Repository(), new BazReturnInvocationHandler());

            interceptor.Save(new Model { Id = 20 });
        }
        [TestMethod]
        public void NoTargetWithInvocationChainInterceptorExample()
        {
            var interceptor = Intercept.WithChain<IRepository, Repository>(new Repository(), builder => {
                builder.Add((next, context) => {
                    context.Invoke();

                    if (context.MemberName == nameof(IRepository.Save))
                    {
                        if (context.ReturnValue is false)
                        {
                            //Log failure
                            return;
                        }
                    }

                    next(context);
                });
            });

            interceptor.Save(new Model { Id = 20 });
        }
        [TestMethod]
        public void TargetWithInvocationChainInterceptorExample()
        {
            var interceptor = Intercept.WithInovcationChain<IRepository>(builder => {
                builder.Add((next, context) => {
                    if (context.MemberName == nameof(IRepository.Save))
                    {
                        context.ReturnValue = true;
                    }

                    // Call next regardless of condition
                    next(context);
                });
            });

            interceptor.Save(new Model { Id = 20 });
        }
        private class BazReturnInvocationHandler : IInvocationHandler
        {
            public void Handle(IInvocationContext context)
            {
                if (context.MemberName == "Baz")
                {
                    context.ReturnValue = context.GetParameter<Model>("model");
                }

                if (context.MemberName == nameof(IRepository.Save))
                {
                    if (context.ReturnValue is false)
                    {
                        //Log failure
                        return;
                    }
                }
            }
        }
    }
}
