using DataCompany.Framework;
using Overmock.Compilation.IL;
using Overmock.Examples.Controllers;
using Overmock.Examples.Storage;

namespace Overmock.Examples.Tests
{
    [TestClass]
    public class UserStoryControllerTests
    {
		private IOvermock<OvermockMethodTemplate> _template;
		private IOvermock<IDataConnection> _connection = new Overmock<IDataConnection>();
        private IOvermock<UserStoryFactory> _factory;
        private IOvermock<IUserStoryService> _service;

        private UserStoryController _controller;

        public UserStoryControllerTests()
        {
            OvermockBuilder.UseBuilder(new ILOvermockBuilder());
        }

        [TestInitialize]
        public void Initialize()
        {
            _template = Overmocked.Setup<OvermockMethodTemplate>();
            _connection = Overmocked.Setup<IDataConnection>();
            _factory = Overmocked.Setup<UserStoryFactory>(args => 
                args.Args(_connection.Target));
            _service = Overmocked.Setup<IUserStoryService>();
        }

        [TestMethod]
        public void TemplateTest()
        {
            _template.Override(t => t.TestMethod(Its.Any<string>())).ToThrow(new Exception("Test"));

            var target = _template.Target;
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