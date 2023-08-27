using FakeCompany.Api.Storage;
using Overmock;

namespace FakeCompany.Api.Tests
{
    public class UserStoryControllerTests
    {
        private readonly IUserStoryService _target = Overmock.Overmock.Interface<IUserStoryService>();

        [Fact]
        public void Test1()
        {
        }
    }
}