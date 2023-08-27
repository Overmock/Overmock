namespace Overmock.Tests.Examples
{
    [TestClass]
    public class ExampleTestsForReadMe
    {
        public class Model
        {
            public int Id { get; set; }
        }
        public interface IRepository
        {
            bool Save(Model model);
        }
        public interface ILog
        {
            void Log(string message);
        }
        public class Service
        {
            private readonly ILog _log;
            private readonly IRepository _repo;
            public Service(ILog log, IRepository repo)
            {
                _log = log;
                _repo = repo;
            }
            public Model SaveModel(Model model)
            {
                try
                {
                    var saved = _repo.Save(model);
                    if (!saved)
                    {
                        _log.Log("Failed to save");
                    }
                    return model;
                }
                catch (Exception ex)
                {
                    _log.Log(ex.Message);
                    throw;
                }
            }
        }

        [TestMethod]
        public void CallsSaveTest()
        {
            var id = 22;
            var wasSaved = false;
            var log = Overmock.AnyInvocation<ILog>();
            var repository = Overmock.Mock<IRepository>()
                .Mock(r => r.Save(Its.Any<Model>()))
                .ToCall(c => {
                    wasSaved = true;
                    return c.Get<Model>("model")?.Id == id;
                }, Times.Once);

            var service = new Service(log, repository.Target);
            service.SaveModel(new Model { Id = id });

            Assert.IsTrue(wasSaved);
        }

        [TestMethod]
        public void ThrowsExceptionWhenSaveFailsTest()
        {
            var expected = "Failed to save";
            var log = Overmock.AnyInvocation<ILog>();
            var repository = Overmock.Mock<IRepository>(r => r.Save(Its.Any<Model>()))
                .ToThrow(new Exception(expected));

            var service = new Service(log, repository.Target);

            try
            {
                service.SaveModel(new Model());

                Assert.Fail("SaveModel Failed to throw an exception.");
            }
            catch (Exception actual)
            {
                Assert.AreEqual(expected, actual.Message);
            }
        }
    }
}
