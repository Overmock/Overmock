using DataCompany.Framework;
using Overmock.Examples.Controllers;
using Overmock.Examples.Storage;

namespace Overmock.Examples.Tests
{
    [TestClass]
    public class UserStoryControllerTests
    {
        private IOvermock<IDataConnection> _connection;
        private IOvermock<UserStoryFactory> _factory;
        private IOvermock<IUserStoryService> _service;

        private UserStoryController _subject;

        [TestInitialize]
        public void Initialize()
        {
            _connection = Overmocked.Setup<IDataConnection>();
            _factory = Overmocked.Setup<UserStoryFactory>(args => 
                args.Args(_connection.Object));
            _service = Overmocked.Setup<IUserStoryService>();
            
            _subject = new UserStoryController(_service.Object);
        }

        [TestMethod]
        public void GetReturnsTheCorrectErrorDetailsWhenAnExceptionHappens()
        {
            // Arrange
            _service.Override(s => s.Get(0)).ToThrow(new Exception("testing"));

            // Act
            var response = _subject.Get();

            // Assert
            Assert.AreEqual(response.ErrorDetails, "testing");
        }
    }
}