using Overmocked.Matchable;
using Overmocked.Tests.Mocks.Methods;

namespace Overmocked.Tests
{
    [TestClass]
    public class MatchTests
    {
        private readonly This<string> _testing = Its.This("testing");

        [TestMethod]
        public void MatchExpressionTest()
        {
            IMatch<int> match = Its.Matching<int>(i => i == 52);

            Assert.IsTrue(match.Matches(52));
        }

        [TestMethod]
        public void DoesntMatchExpressionTest()
        {
            IMatch<int> match = Its.Matching<int>(i => i == 52);

            Assert.IsFalse(match.Matches(2));
        }

        [TestMethod]
        public void ThisTest()
        {
            IMatch<int> match = Its.This<int>(52);

            Assert.IsTrue(match.Matches(52));
        }

        [TestMethod]
        public void ThisAsParameterFieldTest()
        {
            var fifty2 = Its.Matching<int>(p => p == 52);
            var overmock = Overmock.Mock<IMethodsWith2Parameters>();

            //fifty and _testing are "DisplayClass" and "MatchTests" fields.
            overmock.Mock(m => m.VoidMethodWithIntAndString(fifty2, _testing));

            Assert.IsTrue(fifty2.Matches(52));
        }

        [TestMethod]
        public void ThisAsParameterWithLocalVariableTest()
        {
            var fifty2 = Its.Matching<int>(p => p == 52);
            var overmock = Overmock.Mock<IMethodsWith2Parameters>();

            //fifty and _testing are "DisplayClass" and "MatchTests" fields.
            overmock.Mock(m => m.VoidMethodWithIntAndString(fifty2, _testing));

            Assert.IsTrue(fifty2.Matches(52));
        }


        [TestMethod]
        public void ThisDoesntMatchTest()
        {
            IMatch<int> match = Its.This<int>(52);

            Assert.IsFalse(match.Matches(12));
        }

        [TestMethod]
        public void AnyTest()
        {
            IMatch<int> match = Its.Any<int>();

            Assert.IsTrue(match.Matches(52));
        }
    }
}
