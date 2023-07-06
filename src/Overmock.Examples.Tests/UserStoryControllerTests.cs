using DataCompany.Framework;
using Overmock.Examples.Controllers;
using Overmock.Examples.Storage;
using Overmock.Examples.Tests.TestCode;

namespace Overmock.Examples.Tests
{
    [TestClass]
	public class UserStoryControllerTests
	{
		private IOvermock<IDataConnection> _connection = new Overmock<IDataConnection>();
		private IOvermock<ITestInterface> _testInterface = null!;
		private IOvermock<IUserStoryService> _service = null!;
        private UserStoryController? _controller;

		[TestInitialize]
		public void Initialize()
		{
			//_template = Overmocked.Setup<OvermockTemplate>();
			_connection = Overmocked.Setup<IDataConnection>();
            _testInterface = Overmocked.Setup<ITestInterface>();
			_service = Overmocked.Setup<IUserStoryService>();
		}

		[TestMethod]
		public void VoidMethodWithNoParamsTest()
		{
            _testInterface.Override(t => t.VoidMethodWithNoParams());

			var target = _testInterface.Target;

			Assert.IsNotNull(target);

			target.VoidMethodWithNoParams();
		}
		
        [TestMethod]
        public void BoolMethodWithNoParamsTest()
        {
            _testInterface.Override(t => t.BoolMethodWithNoParams())
                .ToReturn(true);

            var target = _testInterface.Target;

            Assert.IsNotNull(target);

            var test = target.BoolMethodWithNoParams();

            Assert.IsTrue(test);
        }

        [TestMethod]
        public void PropertyTest()
        {
            _testInterface.Override(t => t.Name)
                .ToReturn("testing-name");

            var target = _testInterface.Target;

            Assert.IsNotNull(target);

            var test = target.Name;

            Assert.AreEqual("testing-name", test);
        }

        [TestMethod]
		public void GetAllTest()
		{
			var stories = new List<UserStory> { new UserStory() };
			_service.Override(t => t.GetAll())
				.ToCall(c => stories);

			var target = _service.Target;

			Assert.IsNotNull(target);

			var test = target.GetAll();

			Assert.IsNotNull(test);
			Assert.AreEqual(test, stories);
		}

		[TestMethod]
		public void SaveTest()
		{
			var stories = new List<UserStory> { new UserStory() };
			_service.Override(t => t.SaveAll(Any<IEnumerable<UserStory>>.Value))
				.ToReturn(stories);

			var target = _service.Target;

			Assert.IsNotNull(target);

			var test = target.SaveAll(stories);

			Assert.IsNotNull(test);
			Assert.AreEqual(test, stories);
		}

		[TestMethod]
		public void GetReturnsTheCorrectErrorDetailsWhenAnExceptionHappens()
		{
			// Arrange
			_service.Override(s => s.Get(Any<int>.Value))
				.ToThrow(new Exception("testing"));

			_controller = new UserStoryController(_service.Target);

			// Act
			var response = _controller.Get(2);

			// Assert
			Assert.AreEqual(response.ErrorDetails, "testing");
		}

		[TestMethod]
		public void GetReturnsTheResultsWhenPassedTheRightParameters()
		{
			// Arrange
			_connection.Override(s => s.Connect(Its.Any<string>(), Its.Any<string>())).ToCall(c =>
			{
				var user = c.Get<string>("username");
				var password = c.Get<string>(1);

				return true;

			});//.ToReturn(Enumerable.Empty<UserStory>);

			var factory = new UserStoryFactory(_connection.Target);

			// Act;

			// Assert
		}
	}
}