using Overmock.Tests.Examples;

namespace DataCompanyFramework
{
    public interface IDataConnection {}
    public interface IIdentifiable
    {
        int Id { get; }
    }
    public abstract class Entity : IIdentifiable
    {
        protected Entity() { }
        public int Id { get; }
    }
    public abstract class EntityCollection<T> where T : Entity
    {
        protected static readonly object SyncRoot = new object();
        protected EntityCollection(IDataConnection connection) => Connection = connection;
        protected static IList<T> Collection { get; } = new List<T>();
        protected IDataConnection Connection { get; }
        protected abstract T Create(T model);
        public virtual IQueryable<T> AsQueryable() => Collection.AsQueryable();
        public virtual T? Find(int id) => Collection.ElementAt(id);
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
namespace Overmock.Tests.Examples
{
    using DataCompanyFramework;
    public class UserStory : Entity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int? Points { get; set; }
    }
    public interface IUserStoryService
    {
        IEnumerable<UserStory> GetAll();
        UserStory? Get(int id);
        UserStory Save(UserStory model);
    }
    public class UserStoryService : IUserStoryService
    {
        private readonly EntityCollection<UserStory> _collection;
        public UserStoryService(EntityCollection<UserStory> collection) => _collection = collection;
        public UserStory? Get(int id) => _collection.Find(id);
        public IEnumerable<UserStory> GetAll() => _collection.AsQueryable();
        public UserStory Save(UserStory model) => _collection.Upsert(model, original =>
        {
            original.Title = model.Title;
            original.Description = model.Description;
            original.Points = model.Points;
            return original;
        });
    }
    public class UserStoryFactory : EntityCollection<UserStory>
    {
        public UserStoryFactory(IDataConnection connection) : base(connection)
        {
        }
        private static int NextId => Collection.Count;
        protected override UserStory Create(UserStory model)
        {
            lock (SyncRoot)
            {
                var story = new UserStory
                {
                    Id = NextId,
                    Title = model.Title,
                    Description = model.Description,
                    Points = model.Points
                };

                Collection.Add(story);

                return story;
            }
        }
    }
}
namespace ExampleCompany.Controllers
{
    public class Response<T>
    {
        protected Response(Exception errorDetails) => ErrorDetails = errorDetails;
        protected Response(T obj) => Result = obj;
        protected Response(List<T> results) => Results = results;
        public Exception? ErrorDetails { get; set; }
        public T? Result { get; set; }
        public IEnumerable<T>? Results { get; set; }

        public static implicit operator Response<T>(T? obj) => new(obj);

        public static implicit operator Response<T>(List<T> results) => new(results);

        public static implicit operator Response<T>(Exception obj) => new(obj);
    }
    public class UserStoryController
    {
        private readonly IUserStoryService _service;
        public UserStoryController(IUserStoryService service)
        {
            _service = service;
        }
        public Response<UserStory> HttpGet()
        {
            try
            {
                return _service.GetAll().ToList();
            }
            catch (Exception e)
            {
                return e;
            }
        }
        public Response<UserStory> HttpGet(int id)
        {
            try
            {
                return _service.Get(id);
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}
