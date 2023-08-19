
using System;

namespace Kimono.Tests.Proxies
{
	public class Model
	{
		public int Id { get; set; }
	}
	public interface IRepository
	{
		bool Save(Model model);
	}
	public interface IDisposableRepository : IRepository, IDisposable
	{
		public bool IsDisposed { get; }
	}
	public class DisposableRepository : Repository, IDisposableRepository
	{
		public bool IsDisposed { get; private set; }

		public void Dispose()
		{
			IsDisposed = true;
		}
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
        public static void Method()
        {
            Test.Delegate = (object target, object id, object name) => {
                ((IVoidArgsIntString)target).VoidArgsIntString((int)id, (string)name);
            };
        }
	}

    public class Test
    {

        public static MethodArgIntStringReturnsVoid Delegate { get; set; }
    }

    public delegate void MethodArgIntStringReturnsVoid(object target, object id, object name);
}
