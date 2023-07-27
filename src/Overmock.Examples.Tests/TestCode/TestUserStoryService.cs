using System.Reflection;
using Overmock.Examples.Storage;
using Overmock.Runtime.Proxies;

namespace Overmock.Examples.Tests.TestCode
{
    public class TestUserStoryService : Proxy<IUserStoryService>, IUserStoryService
    {
        public TestUserStoryService(IOvermock target) : base(target)
        {
        }

        public IEnumerable<UserStory> GetAll()
        {
            return (IEnumerable<UserStory>)HandleMethodCall((MethodInfo)MethodBase.GetCurrentMethod());
        }

        public UserStory? Get(int id)
        {
            throw new NotImplementedException();
        }

        public UserStory Save(UserStory model)
        {
            throw new NotImplementedException();
        }

        public UserStory Delete(UserStory model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserStory> SaveAll(IEnumerable<UserStory> value)
        {
            throw new NotImplementedException();
        }
    }
}