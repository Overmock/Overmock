namespace DataCompany.Framework
{
	public interface IDataConnection
	{
		bool Connect(string username, string password);
	}
	public class FrameworkDataConnection : IDataConnection
	{
		public bool Connect(string username, string password)
		{
			return true;
		}
	}
	public interface IIdentifiable
	{
		int Id { get; }
	}
	public abstract class Entity : IIdentifiable
	{
		protected Entity() { }
		public int Id { get; set; }
	}
	public abstract class EntityCollection<T> where T : Entity
	{
		protected static readonly object SyncRoot = new object();
		protected EntityCollection(IDataConnection connection) => Connection = connection;
		protected static IList<T> Collection { get; } = new List<T>();
		protected IDataConnection Connection { get; }
		protected abstract T Create(T model);
		public virtual T Delete(T model)
		{
			var result = Collection.FirstOrDefault(m => m.Id == model.Id);

			if (result != null && Collection.Remove(result))
			{
				return result;
			}

			return model;
		}
		public virtual IQueryable<T> AsQueryable() => Collection.AsQueryable();
		public virtual T? Find(int id) => Collection.ElementAtOrDefault(id);
		public virtual T Upsert(T model, Func<T, T> update)
		{
			var story = Find(model.Id);

			if (story == null)
			{
				return Create(model);
			}

			return update(story);
		}
	}
}