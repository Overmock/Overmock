using DataCompany.Framework;
using FakeCompany.Api.Controllers;
using FakeCompany.Api.Storage;
using Overmocked;

namespace FakeCompany.Api.Tests
{
    public class UserStoryServiceTests
    {
        private readonly IOvermock<IUserStoryFactory<UserStory>> _serviceMock = Overmock.Mock<IUserStoryFactory<UserStory>>();
        private readonly List<UserStory> _userStories = new List<UserStory>();
        private readonly UserStory _userStory = new UserStory();

        public UserStoryServiceTests()
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
            Overmock.Mock(_serviceMock, m => m.AsQueryable())
                .ToReturn(_userStories.AsQueryable());

            var controller = new UserStoryService(_serviceMock.Target);
            var response = controller.GetAll();

            Assert.NotNull(response);
            Assert.Single(response);
            Assert.Collection(response, us => Assert.Equal(_userStory.Id, us.Id));
        }

        [Fact]
        public void GetByIdReturnsTheCorrectUserStory()
        {
            Overmock.Mock(_serviceMock, m => m.Find(52))
                .ToReturn(_userStory);

            var controller = new UserStoryService(_serviceMock.Target);
            var response = controller.Get(52);

            Assert.NotNull(response);
            Assert.Equal(_userStory.Id, response.Id);
        }

        [Fact]
        public void GetByIdReturnsNullUserStoryNotFound()
        {
            Overmock.Mock(_serviceMock, m => m.Find(20))
                .ToReturn((UserStory)null!);

            var controller = new UserStoryService(_serviceMock.Target);
            var response = controller.Get(20);

            Assert.Null(response);
        }

        [Fact]
        public void PostReturnsTheCorrectUserStory()
        {
            Overmock.Mock(_serviceMock, m => m.Upsert(Its.This(_userStory), Its.Any<Func<UserStory, UserStory>>()))
                .ToReturn(_userStory);

            var controller = new UserStoryService(_serviceMock.Target);
            var response = controller.Save(_userStory);

            Assert.NotNull(response);
            Assert.Equal(_userStory.Id, response.Id);
        }

        [Fact]
        public void DeleteReturnsSuccessWhenDeletingTheUserStoryIsSuccessful()
        {
            Overmock.Mock(_serviceMock, m => m.Delete(Its.Any<UserStory>()))
                .ToReturn(true);

            var controller = new UserStoryService(_serviceMock.Target);
            var response = controller.Delete(_userStory);

            Assert.True(response);
        }
    }
}