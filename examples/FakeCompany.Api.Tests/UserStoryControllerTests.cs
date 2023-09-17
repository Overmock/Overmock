using FakeCompany.Api.Controllers;
using FakeCompany.Api.Storage;
using Overmocked;

namespace FakeCompany.Api.Tests
{
    public class UserStoryControllerTests
    {
        private readonly IOvermock<IUserStoryService> _serviceMock = Overmock.Mock<IUserStoryService>();
        private readonly List<UserStory> _userStories = new List<UserStory>();
        private readonly UserStory _userStory = new UserStory();

        public UserStoryControllerTests()
        {
            _userStory.Id = 52;
            _userStory.Title = "- test -";
            _userStory.Points = 5252;
            _userStory.Description = "- description -";
            _userStories.Add(_userStory);
        }

        [Fact]
        public void GetReturnsAllUserStories()
        {
            Overmock.Mock(_serviceMock, m => m.GetAll())
                .ToReturn(_userStories);

            var controller = new UserStoryController(_serviceMock.Target);
            var response = controller.Get();

            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Single(response.Results);
        }

        [Fact]
        public void GetByIdReturnsTheCorrectUserStory()
        {
            Overmock.Mock(_serviceMock, m => m.Get(Its.Any<int>()))
                .ToReturn(_userStory);

            var controller = new UserStoryController(_serviceMock.Target);
            var response = controller.Get(52);

            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal(52, response.Result.Id);
        }

        [Fact]
        public void PostReturnsTheCorrectUserStory()
        {
            Overmock.Mock(_serviceMock, m => m.Save(Its.Any<UserStory>()))
                .ToReturn(_userStory);

            var controller = new UserStoryController(_serviceMock.Target);
            var response = controller.Post(_userStory);

            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal(52, response.Result.Id);
        }

        [Fact]
        public void DeleteReturnsSuccessWhenDeletingTheUserStoryIsSuccessful()
        {
            Overmock.Mock(_serviceMock, m => m.Delete(Its.Any<UserStory>()))
                .ToReturn(true);

            var controller = new UserStoryController(_serviceMock.Target);
            var response = controller.Delete(_userStory);

            Assert.NotNull(response);
            Assert.True(response.Success);
        }
    }
}