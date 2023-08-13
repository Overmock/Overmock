# Kimono

![DOTNET Build](https://github.com/overmock/overmock/actions/workflows/dotnet.yml/badge.svg)

<pre>
 _    _                             
| |  (_)                            
| | ___ _ __ ___   ___  _ __   ___  
| |/ / | '_ ` _ \ / _ \| '_ \ / _ \ 
|   <| | | | | | | (_) | | | | (_) |
|_|\_\_|_| |_| |_|\___/|_| |_|\___/
</pre>


Kimono is a dynamic proxy framework that allows intercepting methods and properties. Currently in development! Working on generating MSIL for out and ref parameters definitions. See below for examples.

``` C#
	public class Program
	{
		public void Main(string[] args)
		{
			var examples = new KimonoExamples();
			examples.NoTargetWithCallbackInterceptorExample();
			examples.TargetWithCallbackInterceptorExample();
			examples.NoTargetWithHandlersInterceptorExample();
			examples.TargetWithHandlersInterceptorExample();
			examples.NoTargetWithInvocationChainInterceptorExample();
			examples.TargetWithInvocationChainInterceptorExample();
		}
	}
	public class KimonoExamples
	{
		public void NoTargetWithCallbackInterceptorExample()
		{
			var interceptor = Interceptor.WithCallback<IRepository>(context =>
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
			var interceptor = Interceptor.TargetedWithCallback<IRepository, Repository>(new Repository(), context =>
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
			var interceptor = Interceptor.WithHandlers<IRepository>(new BazReturnInvocationHandler());

			interceptor.Save(new Model { Id = 20 });
		}
		public void TargetWithHandlersInterceptorExample()
		{
			var interceptor = Interceptor.TargetedWithHandlers<IRepository, Repository>(new Repository(), new BazReturnInvocationHandler());

			interceptor.Save(new Model { Id = 20 });
		}
		public void NoTargetWithInvocationChainInterceptorExample()
		{
			var interceptor = Interceptor.TargetedWithInovcationChain<IRepository, Repository>(new Repository(), builder =>
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
			var interceptor = Interceptor.WithInovcationChain<IRepository>(builder =>
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
```
