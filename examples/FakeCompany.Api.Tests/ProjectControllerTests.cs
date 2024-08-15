using FakeCompany.Api.Controllers;
using FakeCompany.Api.Storage;
using Overmocked;
using Xunit;

namespace FakeCompany.Api.Tests
{
    public class ProjectControllerTests
    {
        private readonly IOvermock<IProjectService> _serviceMock = Overmock.Mock<IProjectService>();
        private readonly List<Project> _projects = new List<Project>();
        private readonly Project _project = new Project();

        public ProjectControllerTests()
        {
            _project.Id = 101;
            _project.Name = "Test Project";
            _project.Description = "Test Project Description";
            _projects.Add(_project);
        }

        [Fact]
        public void GetReturnsAllProjects()
        {
            Overmock.Mock(_serviceMock, m => m.GetAll())
                .ToReturn(_projects);

            var controller = new ProjectController(_serviceMock.Target);
            var response = controller.Get();

            Assert.NotNull(response);
            Assert.Single(response.Results);
        }

        [Fact]
        public void GetByIdReturnsTheCorrectProject()
        {
            Overmock.Mock(_serviceMock, m => m.Get(Its.Any<int>()))
                .ToReturn(_project);

            var controller = new ProjectController(_serviceMock.Target);
            var response = controller.Get(101);

            Assert.NotNull(response);
            Assert.Equal(101, response.Result.Id);
        }

        [Fact]
        public void PostReturnsTheCorrectProject()
        {
            Overmock.Mock(_serviceMock, m => m.Save(Its.Any<Project>()))
                .ToReturn(_project);

            var controller = new ProjectController(_serviceMock.Target);
            var response = controller.Post(_project);

            Assert.NotNull(response);
            Assert.Equal(101, response.Result.Id);
        }

        [Fact]
        public void DeleteReturnsSuccessWhenDeletingTheProjectIsSuccessful()
        {
            Overmock.Mock(_serviceMock, m => m.Delete(Its.Any<Project>()))
                .ToReturn(true);

            var controller = new ProjectController(_serviceMock.Target);
            var response = controller.Delete(_project);

            Assert.NotNull(response);
        }
    }
}
