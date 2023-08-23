namespace Overmock.Tests.SimpleMock
{
	//[TestClass]
	//public class UserStoryControllerTests
	//{
	//	private IUserStoryService _service;

	//	private UserStoryController _controller;

	//	public UserStoryControllerTests()
	//	{

	//		var stub = Stub.Interface<IUserStoryService>();
	//	}

	//	[TestInitialize]
	//	public void Initialize()
	//	{
	//		_service = Mock.Interface<IUserStoryService>();
	//	}

	//	[TestMethod]
	//	public void GetTest()
	//	{
	//		//var story = new UserStory();
	//		//_service.Override(t => t.Get(Its.Any<int>()))
	//		//	.ToReturn(story);

	//		//var target = _service.Target;

	//		//Assert.IsNotNull(target);

	//		//var test = target.Get(123);

	//		//Assert.IsNotNull(test);
	//		//Assert.AreEqual(test, story);
	//	}

	//	//[TestMethod]
	//	//public void GetAllTest()
	//	//{
	//	//	var stories = new List<UserStory> { new UserStory() };
	//	//	_service.Override(t => t.GetAll())
	//	//		.ToCall(c =>
	//	//		{
	//	//			return stories;
	//	//		});

	//	//	var target = _service.Target;

	//	//	Assert.IsNotNull(target);

	//	//	var test = target.GetAll();

	//	//	Assert.IsNotNull(test);
	//	//	Assert.AreEqual(test, stories);
	//	//}

	//	//[TestMethod]
	//	//public void SaveTest()
	//	//{
	//	//	var stories = new List<UserStory> { new UserStory() };
	//	//	_service.Override(t => t.SaveAll(Any<IEnumerable<UserStory>>.Value))
	//	//		.ToReturn(stories);

	//	//	var target = _service.Target;

	//	//	Assert.IsNotNull(target);

	//	//	var test = target.SaveAll(stories);

	//	//	Assert.IsNotNull(test);
	//	//	Assert.AreEqual(test, stories);
	//	//}

	//	//[TestMethod]
	//	//public void GetReturnsTheCorrectErrorDetailsWhenAnExceptionHappens()
	//	//{
	//	//	// Arrange
	//	//	_service.Override(s => s.Get(Any<int>.Value))
	//	//		.ToThrow(new Exception("testing"));

	//	//	_controller = new UserStoryController(_service.Target);

	//	//	// Act
	//	//	var response = _controller.Get(2);

	//	//	// Assert
	//	//	Assert.AreEqual(response.ErrorDetails, "testing");
	//	//}

	//	//[TestMethod]
	//	//public void GetReturnsTheResultsWhenPassedTheRightParameters()
	//	//{
	//	//	// Arrange
	//	//	_connection.Override(s => s.Connect(Its.Any<string>(), Its.Any<string>())).ToCall(c =>
	//	//	{
	//	//		var user = c.Get<string>("username");
	//	//		var password = c.Get<string>(1);

	//	//		return true;

	//	//	});//.ToReturn(Enumerable.Empty<UserStory>);

	//	//	var factory = new UserStoryFactory(_connection.Target);

	//	//	// Act;

	//	//	// Assert
	//	//}
	//}
}