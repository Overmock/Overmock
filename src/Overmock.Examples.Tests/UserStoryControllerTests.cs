using DataCompany.Framework;
using Overmock.Examples.Controllers;
using Overmock.Examples.Storage;

namespace Overmock.Examples.Tests
{
    [TestClass]
    public class UserStoryControllerTests
    {
        private IOvermock<IDataConnection> _connection = new Overmock<IDataConnection>();
        private IOvermock<UserStoryFactory> _factory;
        private IOvermock<IUserStoryService> _service;

        private UserStoryController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _connection = Overmocked.Setup<IDataConnection>();
            _factory = Overmocked.Setup<UserStoryFactory>(args => 
                args.Args(_connection.Target));
            _service = Overmocked.Setup<IUserStoryService>();
        }

        [TestMethod]
        public void GetReturnsTheCorrectErrorDetailsWhenAnExceptionHappens()
        {
            // Arrange
            _service.Override(s => s.Get(0)).ToThrow(new Exception("testing"));

            _controller = new UserStoryController(_service.Target);

            // Act
            var response = _controller.Get();

            // Assert
            //Assert.AreEqual(response.ErrorDetails, "testing");
        }

        [TestMethod]
        public void GetReturnsTheResultsWhenPassedTheRightParameters()
        {
            // Arrange
            _service.Override(s => s.GetAll()).ToCall(c =>
            {
                var id = c.Get<int>("id");

                return Enumerable.Empty<UserStory>();

            }).ToReturn(Enumerable.Empty<UserStory>);

            _controller = new UserStoryController(_service.Target);

            // Act
            var response = _controller.Get();

            // Assert
            //Assert.AreEqual(response.ErrorDetails, "testing");
        }
    }
}