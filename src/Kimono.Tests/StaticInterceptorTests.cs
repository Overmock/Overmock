
using Kimono.Tests.Examples;

namespace Kimono.Tests
{
	[TestClass]
	public class StaticInterceptorTests
	{
		[TestMethod]
		public void WithCallbackInterceptorTest()
		{
			var called = false;
			var bazCalled = false;

			var interceptorWithCallback = Interceptor.WithCallback<IFoo>(context => {
				called = true;
				if (context.MemberName == "Baz")
				{
					bazCalled = true;
				}
			});

			interceptorWithCallback.Bar();

			Assert.IsTrue(called);
			Assert.IsFalse(bazCalled);
		}

		[TestMethod]
		public void TargetedWithCallbackInterceptorTest()
		{
			var called = false;
			var bazCalled = true;

			var interceptorWithCallback = Interceptor.TargetedWithCallback<IFoo, Foo>(new Foo(), context => {
				called = true;
				if (context.MemberName == "Bar")
				{
					bazCalled = false;
				}
			});

			interceptorWithCallback.Bar();

			Assert.IsTrue(called);
			Assert.IsFalse(bazCalled);
		}

		[TestMethod]
		public void TargetedWithHandlersInterceptorTest()
		{
			var interceptorWithCallback = Interceptor.TargetedWithHandlers<IFoo, Foo>(new Foo(), new TestHandler());

			var baz = interceptorWithCallback.Baz("a-test", "b-test");

			Assert.AreEqual("a-test", baz.A);
			Assert.AreEqual("b-test", baz.B);
		}

		[TestMethod]
		public void WithHandlersInterceptorTest()
		{
			var interceptorWithCallback = Interceptor.WithHandlers<IFoo>(new TestHandler());

			var baz = interceptorWithCallback.Baz("a-test", "b-test");

			Assert.AreEqual("a-test", baz.A);
			Assert.AreEqual("b-test", baz.B);
		}


		[TestMethod]
		public void WithHandlersEnumerableInterceptorTest()
		{
			var list = new List<IInvocationHandler>() { new TestHandler() };
			var interceptorWithCallback = Interceptor.WithHandlers<IFoo>(list);

			var baz = interceptorWithCallback.Baz("a-test", "b-test");

			Assert.AreEqual("a-test", baz.A);
			Assert.AreEqual("b-test", baz.B);
		}

		[TestMethod]
		public void WithInovcationChainInterceptorTest()
		{
			var barCalled = false;
			var bazCalled = false;

			var interceptorWithCallback = Interceptor.WithInovcationChain<IFoo>(builder => {
				builder.Add((next, context) => {
					if (context.MemberName == "Bar")
					{
						barCalled = true;
					}

					next(context);
				})
				.Add((next, context) => {
					if (context.MemberName == "Baz")
					{
						bazCalled = true;
					}
				});
			});

			interceptorWithCallback.Bar();
			interceptorWithCallback.Baz("a-test", "b-test");

			Assert.IsTrue(barCalled);
			Assert.IsTrue(bazCalled);
		}

		[TestMethod]
		public void TargetedWithInovcationChainInterceptorTest()
		{
			var barCalled = false;
			var bazCalled = false;

			var interceptorWithCallback = Interceptor.TargetedWithInovcationChain<IFoo, Foo>(new Foo(), builder => {
				builder.Add((next, context) => {
					if (context.MemberName == "Bar")
					{
						barCalled = true;
					}

					next(context);
				})
				.Add((next, context) => {
					if (context.MemberName == "Baz")
					{
						bazCalled = true;
					}

					next(context);
				});
			});

			interceptorWithCallback.Bar();
			interceptorWithCallback.Baz("a-test", "b-test");

			Assert.IsTrue(barCalled);
			Assert.IsTrue(bazCalled);
		}


		[TestMethod]
		public void TargetedWithInovcationChainInterceptorDoesNotCallNextTest()
		{
			var barCalled = false;
			var bazCalled = false;

			var interceptorWithCallback = Interceptor.TargetedWithInovcationChain<IFoo, Foo>(new Foo(), builder => {
				builder.Add((next, context) => {
					if (context.MemberName == "Bar")
					{
						barCalled = true;
					}
				})
				.Add((next, context) => {
					if (context.MemberName == "Baz")
					{
						bazCalled = true;
					}
				});
			});

			interceptorWithCallback.Bar();
			interceptorWithCallback.Baz("a-test", "b-test");

			Assert.IsTrue(barCalled);
			Assert.IsFalse(bazCalled);
		}

		private class TestHandler : IInvocationHandler
		{
			public void Handle(IInvocationContext context)
			{
				if (context.MemberName == "Baz")
				{
					context.ReturnValue = new Baz
					{
						A = context.Parameters.Get<string>("a"),
						B = context.Parameters.Get<string>("b")
					};
				}
			}
		}
	}
}
