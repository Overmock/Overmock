﻿using Kimono.Tests.Proxies;

namespace Kimono.Tests.Examples
{
	public class KimonoExamples
	{
		public void NoTargetWithCallbackInterceptorExample()
		{
			var interceptor = Intercept.WithCallback<IRepository>(context =>
			{
				if (context.MemberName == "Baz")
				{
					context.ReturnValue = context.GetParameter<Model>("model");
				}

				if (context.MemberName == nameof(IRepository.Save))
				{
					context.InvokeTarget();
				}
			});

			interceptor.Save(new Model { Id = 20 });
		}
		public void TargetWithCallbackInterceptorExample()
		{
			var interceptor = Intercept.TargetedWithCallback<IRepository, Repository>(new Repository(), context =>
			{
				context.InvokeTarget();

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
		public void NoTargetWithHandlersInterceptorExample()
		{
			var interceptor = Intercept.WithHandlers<IRepository>(new BazReturnInvocationHandler());

			interceptor.Save(new Model { Id = 20 });
		}
		public void TargetWithHandlersInterceptorExample()
		{
			var interceptor = Intercept.TargetedWithHandlers<IRepository, Repository>(new Repository(), new BazReturnInvocationHandler());

			interceptor.Save(new Model { Id = 20 });
		}
		public void NoTargetWithInvocationChainInterceptorExample()
		{
			var interceptor = Intercept.TargetedWithInovcationChain<IRepository, Repository>(new Repository(), builder =>
			{
				builder.Add((next, context) =>
				{
					context.InvokeTarget();

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
		public void TargetWithInvocationChainInterceptorExample()
		{
			var interceptor = Intercept.WithInovcationChain<IRepository>(builder =>
			{
				builder.Add((next, context) =>
				{
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
