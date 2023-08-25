using Overmock.Tests.Mocks;
using Overmock.Tests.Mocks.Properties;

namespace Overmock.Tests.OvermockedMock
{
    [TestClass]
    public partial class PropertyGetTests
    {
        private readonly IPropertiesWithGet _overmock = Over.MockInterface<IPropertiesWithGet>();

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
