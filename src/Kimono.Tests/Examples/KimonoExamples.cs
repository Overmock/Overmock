
namespace Kimono.Tests.Examples
{
    public class KimonoExamples
    {
        public void NoTargetWithCallbackInterceptorExample()
        {
            var interceptorWithCallback = Interceptor.WithCallback<IFoo>(context => {
				if (context.MemberName == "Baz") {
					context.ReturnValue = new Baz {
						A = context.Parameters.Get<string>("a"),
						B = context.Parameters.Get<string>("b")
					};
				}
            });

            interceptorWithCallback.Bar();
		}
		public void TargetWithCallbackInterceptorExample()
		{
			var interceptorWithCallback = Interceptor.TargetedWithCallback<IFoo, Foo>(new Foo(), context => {
				if (context.MemberName == "Baz") {
					context.ReturnValue = new Baz {
						A = context.Parameters.Get<string>("a"),
						B = context.Parameters.Get<string>("b")
					};
				}
			});

			interceptorWithCallback.Bar();
		}
		public void NoTargetWithHandlersInterceptorExample()
		{
			var interceptorWithCallback = Interceptor.WithHandlers<IFoo>(new BazReturnInvocationHandler());

			interceptorWithCallback.Bar();
		}
		public void TargetWithHandlersInterceptorExample()
		{
			var interceptorWithCallback = Interceptor.TargetedWithHandlers<IFoo, Foo>(new Foo(), new BazReturnInvocationHandler());

			interceptorWithCallback.Bar();
		}
		public void NoTargetWithInvocationChainInterceptorExample()
		{
			var interceptorWithCallback = Interceptor.TargetedWithInovcationChain<IFoo, Foo>(new Foo(), builder => {
				builder.Add((next, context) => {
					if (context.MemberName == "Baz") {
						context.ReturnValue = new Baz {
							A = context.Parameters.Get<string>("a"),
							B = context.Parameters.Get<string>("b")
						};
					}

					next(context);
				});
			});

			interceptorWithCallback.Bar();
		}
		public void TargetWithInvocationChainInterceptorExample()
		{
			var interceptorWithCallback = Interceptor.WithInovcationChain<IFoo>(builder => {
				builder.Add((next, context) => {
					if (context.MemberName == "Baz") {
						context.ReturnValue = new Baz {
							A = context.Parameters.Get<string>("a"),
							B = context.Parameters.Get<string>("b")
						};
					}

					next(context);
				});
			});

			interceptorWithCallback.Bar();
		}
		private class BazReturnInvocationHandler : IInvocationHandler
		{
			public void Handle(IInvocationContext context)
			{
				if (context.MemberName == "Baz") {
					context.ReturnValue = new Baz {
						A = context.Parameters.Get<string>("a"),
						B = context.Parameters.Get<string>("b")
					};
				}
			}
		}
	}
    public interface IFoo
    {
        void Bar();

        Baz Baz(string a, string b);
    }
	public class Foo : IFoo
	{
		public void Bar()
		{
		}

		public Baz Baz(string a, string b)
		{
			return new Baz { A = a, B = b };
		}
	}
	public class Baz
	{
        public string A { get; set; }
        public string B { get; set; }
	}
}
