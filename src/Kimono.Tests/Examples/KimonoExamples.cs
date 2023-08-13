
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
	public class Model
	{
		public int Id { get; set; }
	}
	public interface IRepository
	{
		bool Save(Model model);
	}
	public class Repository : IRepository
	{
		public bool Save(Model model)
		{
			return true;
		}
	}
	public interface ILog
	{
		void Log(string message);
	}
	public class Service
	{
		private readonly ILog _log;
		private readonly IRepository _repo;
		public Service(ILog log, IRepository repo)
		{
			_log = log;
			_repo = repo;
		}
		public Model SaveModel(Model model)
		{
			try
			{
				var saved = _repo.Save(model);
				if (!saved)
				{
					_log.Log("Failed to save");
				}
				return model;
			}
			catch (Exception ex)
			{
				_log.Log(ex.Message);
				throw;
			}
		}
	}
}
