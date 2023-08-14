
using Kimono.Tests.Examples;
using Kimono.Tests.Proxies;
using System;

namespace Kimono.Tests
{
	[TestClass]
	public class StaticInterceptorTests
	{
		[TestMethod]
		public void WithCallbackInterceptorTest()
		{
			var called = false;
			var saveCalled = false;

			var interceptor = Intercept.WithCallback<IRepository>(context => {
				called = true;
				if (context.MemberName == "Save")
				{
					saveCalled = true;
				}
			});

			var returnedTrue = interceptor.Save(new Model { Id = 69 });

			Assert.IsFalse(returnedTrue);
			Assert.IsTrue(called);
			Assert.IsTrue(saveCalled);
		}

		[TestMethod]
		public void WithHandlersInterceptorTest()
		{
			var interceptor = Intercept.WithHandlers<IRepository>(new TestHandler());

			var returnedTrue = interceptor.Save(new Model { Id = 69 });

			Assert.IsTrue(returnedTrue);
		}

		[TestMethod]
		public void WithHandlersEnumerableInterceptorTest()
		{
			var list = new List<IInvocationHandler>() { new TestHandler() };
			var interceptor = Intercept.WithHandlers<IRepository>(list);

			var returnedTrue = interceptor.Save(new Model { Id = 69 });

			Assert.IsTrue(returnedTrue);
		}

		[TestMethod]
		public void WithInovcationChainInterceptorTest()
		{
			var called = false;
			var saveCalled = false;

			var interceptor = Intercept.WithInovcationChain<IRepository>(builder => {
				builder.Add((next, context) => {
					called = true;

					next(context);
				})
				.Add((next, context) => {
					if (context.MemberName == "Save")
					{
						saveCalled = true;
						context.ReturnValue = saveCalled;
					}
				});
			});

			var returnedTrue = interceptor.Save(new Model { Id = 69 });

			Assert.IsTrue(returnedTrue);
			Assert.IsTrue(called);
			Assert.IsTrue(saveCalled);
		}

		[TestMethod]
		public void TargetedWithCallbackInterceptorTest()
		{
			var called = false;
			var saveCalled = false;

			var interceptor = Intercept.TargetedWithCallback<IRepository, Repository>(new Repository(), context => {
				called = true;
				if (context.MemberName == "Save")
				{
					saveCalled = true;
					context.ReturnValue = true;
				}
			});

			var returnedTrue = interceptor.Save(new Model { Id = 69 });

			Assert.IsTrue(returnedTrue);
			Assert.IsTrue(called);
			Assert.IsTrue(saveCalled);
		}

		[TestMethod]
		public void TargetedWithHandlersInterceptorTest()
		{
			var interceptor = Intercept.TargetedWithHandlers<IRepository, Repository>(new Repository(), new TestHandler());

			var returnedTrue = interceptor.Save(new Model { Id = 69 });

			Assert.IsTrue(returnedTrue);
		}

		[TestMethod]
		public void TargetedWithInovcationChainInterceptorTest()
		{
			var firstCalled = false;
			var secondCalled = false;

			var interceptor = Intercept.TargetedWithInovcationChain<IRepository, Repository>(new Repository(), builder => {
				builder.Add((next, context) => {
					firstCalled = true;
					if (!secondCalled)
					{
						next(context);
					}
				})
				.Add((next, context) => {
					secondCalled = true;
					context.ReturnValue = true;
				});
			});

			var returnedTrue = interceptor.Save(new Model { Id = 69 });

			Assert.IsTrue(returnedTrue);
			Assert.IsTrue(firstCalled);
			Assert.IsTrue(secondCalled);
		}

		[TestMethod]
		public void TargetedWithInovcationChainInterceptorDoesNotCallNextTest()
		{
			var firstCalled = false;
			var secondCalled = false;

			var interceptor = Intercept.TargetedWithInovcationChain<IRepository, Repository>(new Repository(), builder => {
				builder.Add((next, context) => {
					firstCalled = true;
				})
				.Add((next, context) => {
					secondCalled = true;
					context.ReturnValue = true;
				});
			});

			var returnedTrue = interceptor.Save(new Model { Id = 69 });

			Assert.IsFalse(returnedTrue);
			Assert.IsTrue(firstCalled);
			Assert.IsFalse(secondCalled);
		}

		[TestMethod]
		public void DisposableTargetedWithCallbackInterceptorTest()
		{
			var called = false;
			var saveCalled = false;

			using (var interceptor = Intercept.DisposableTargetedWithCallback<IDisposableRepository, DisposableRepository>(new DisposableRepository(), context => {
				called = true;
				if (context.MemberName == "Save")
				{
					saveCalled = true;
					context.ReturnValue = true;
				}
			}))
			{
				var returnedTrue = interceptor.Save(new Model { Id = 69 });

				Assert.IsTrue(returnedTrue);
				Assert.IsTrue(called);
				Assert.IsTrue(saveCalled);
			}
		}

		[TestMethod]
		public void DisposableTargetedWithHandlersInterceptorTest()
		{
			var interceptor = Intercept.DisposableTargetedWithHandlers<IDisposableRepository, DisposableRepository>(new DisposableRepository(), new TestHandler());

			var returnedTrue = interceptor.Save(new Model { Id = 69 });

			Assert.IsTrue(returnedTrue);
		}

		[TestMethod]
		public void DisposableTargetedWithInovcationChainInterceptorTest()
		{
			var firstCalled = false;
			var secondCalled = false;

			var interceptor = Intercept.DisposableTargetedWithInovcationChain<IDisposableRepository, DisposableRepository>(new DisposableRepository(), builder => {
				builder.Add((next, context) => {
					firstCalled = true;
					if (!secondCalled)
					{
						next(context);
					}
				})
				.Add((next, context) => {
					secondCalled = true;
					context.ReturnValue = true;
				});
			});

			var returnedTrue = interceptor.Save(new Model { Id = 69 });

			Assert.IsTrue(returnedTrue);
			Assert.IsTrue(firstCalled);
			Assert.IsTrue(secondCalled);
		}

		[TestMethod]
		public void DisposableTargetedWithInovcationChainInterceptorDoesNotCallNextTest()
		{
			var firstCalled = false;
			var secondCalled = false;

			var interceptor = Intercept.DisposableTargetedWithInovcationChain<IDisposableRepository, DisposableRepository>(new DisposableRepository(), builder => {
				builder.Add((next, context) => {
					firstCalled = true;
				})
				.Add((next, context) => {
					secondCalled = true;
					context.ReturnValue = true;
				});
			});

			var returnedTrue = interceptor.Save(new Model { Id = 69 });

			Assert.IsFalse(returnedTrue);
			Assert.IsTrue(firstCalled);
			Assert.IsFalse(secondCalled);
		}

		private class TestHandler : IInvocationHandler
		{
			public void Handle(IInvocationContext context)
			{
				if (context.MemberName == "Save")
				{
					context.ReturnValue = true;
				}
			}
		}
	}
}
