using DataCompany.Framework;

namespace Overmock.Examples.Storage
{
    public class UserStory : Entity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int? Points { get; set; }
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
    public interface IUserStoryService
    {
        IEnumerable<UserStory> GetAll();
        UserStory? Get(int id);
        UserStory Save(UserStory model);
        UserStory Delete(UserStory model);
    }
    public class UserStoryService : IUserStoryService
    {
        private readonly EntityCollection<UserStory> _collection;
        public UserStoryService(EntityCollection<UserStory> collection) => _collection = collection;

        public UserStory Delete(UserStory model)
        {
            return _collection.Delete(model);
        }

        public UserStory? Get(int id) => _collection.Find(id);
        public IEnumerable<UserStory> GetAll() => _collection.AsQueryable();
        public UserStory Save(UserStory model) => _collection.Upsert(model, original => {
            original.Title = model.Title;
            original.Description = model.Description;
            original.Points = model.Points;
            return original;
        });
    }
}
