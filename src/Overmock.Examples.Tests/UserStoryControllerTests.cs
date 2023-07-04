using DataCompany.Framework;
using Overmock.Compilation;
using Overmock.Compilation.IL;
using Overmock.Examples.Controllers;
using Overmock.Examples.Storage;

namespace Overmock.Examples.Tests
{
	[TestClass]
	public class UserStoryControllerTests
	{
		private IOvermock<OvermockTemplate> _template;
		private IOvermock<IDataConnection> _connection = new Overmock<IDataConnection>();
		private IOvermock<UserStoryFactory> _factory;
		private IOvermock<IUserStoryService> _service;

		private UserStoryController _controller;

		public UserStoryControllerTests()
		{
			OvermockSetup.UseIlBuilder();
		}

		[TestInitialize]
		public void Initialize()
		{
			//_template = Overmocked.Setup<OvermockTemplate>();
			_connection = Overmocked.Setup<IDataConnection>();
			//_factory = Overmocked.Setup<UserStoryFactory>(args =>
			//	args.Args(_connection.Target));
			_service = Overmocked.Setup<IUserStoryService>();
		}

		[TestMethod]
		public void GetTest()
		{
			var story = new UserStory();
			_service.Override(t => t.Get(Its.Any<int>()))
				.ToReturn(story);

			var target = _service.Target;

			Assert.IsNotNull(target);

			var test = target.Get(123);

			Assert.IsNotNull(test);
			Assert.AreEqual(test, story);
		}

		[TestMethod]
		public void GetAllTest()
		{
			var stories = new List<UserStory> { new UserStory() };
			_service.Override(t => t.GetAll())
				.ToReturn(stories);

			var target = _service.Target;

			Assert.IsNotNull(target);

			var test = target.GetAll();

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