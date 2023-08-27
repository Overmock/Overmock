using Overmocked.Tests.Mocks;
using Overmocked.Tests.Mocks.Properties;

namespace Overmocked.Tests
{
    [TestClass]
    public partial class PropertyGetTests
    {
        private readonly IOvermock<IPropertiesWithGet> _overmock = Overmock.Mock<IPropertiesWithGet>();

        private readonly Model _model1 = new Model();
        private readonly Model _model2 = new Model();
        private readonly List<Model> _models = new List<Model>();

        [TestInitialize]
        public void Initialize()
        {
            _models.Add(_model1);
            _models.Add(_model2);
        }
    }
}
