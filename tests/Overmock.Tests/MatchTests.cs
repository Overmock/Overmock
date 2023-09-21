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
        public void ThisAsParameterAndThisFieldTest()
        {
            var matchesTrue = Its.Matching<bool>(p => p == true);
            var overmock = Overmock.Mock<IMethodsWith2Parameters>();
            overmock.Mock(m => m.ListOfModelMethodWithNoParams(matchesTrue, 52.02m))
                .ToBeCalled();

            overmock.Target.ListOfModelMethodWithNoParams(true, 52.02m);

            Assert.IsTrue(matchesTrue.Matches(true));
            Assert.IsFalse(matchesTrue.Matches(false));
        }

        [TestMethod]
        public void ThisAsParameterWithLocalVariableTest()
        {
            var fifty2 = Its.Matching<int>(p => p == 52);
            var overmock = Overmock.Mock<IMethodsWith2Parameters>();
            overmock.Mock(m => m.VoidMethodWithIntAndString(fifty2, _testing))
                .ToBeCalled();

            overmock.Target.VoidMethodWithIntAndString(52, "testing");

            Assert.IsTrue(fifty2.Matches(52));
            Assert.IsTrue(_testing.Matches("testing"));
        }

        [TestMethod]
        public void ThisAsParameterAndThisFieldTestThrowsExceptionWhenPassedWrongValues()
        {
            var matchesTrue = Its.Matching<bool>(p => p == true);
            var overmock = Overmock.Mock<IMethodsWith2Parameters>();
            overmock.Mock(m => m.ListOfModelMethodWithNoParams(matchesTrue, 52.02m))
                .ToBeCalled();

            Assert.ThrowsException<OvermockException>(() =>
                overmock.Target.ListOfModelMethodWithNoParams(false, 52.02m));

            Assert.IsTrue(matchesTrue.Matches(true));
            Assert.IsFalse(matchesTrue.Matches(false));
        }

        [TestMethod]
        public void LocalParameterAndThisFieldTestThrowsExceptionWhenPassedWrongValues()
        {
            var matchesTrue = Its.Matching<bool>(p => p == true);
            var overmock = Overmock.Mock<IMethodsWith2Parameters>();
            overmock.Mock(m => m.ListOfModelMethodWithNoParams(matchesTrue, 52.02m))
                .ToBeCalled();

            Assert.ThrowsException<OvermockException>(() =>
                overmock.Target.ListOfModelMethodWithNoParams(true, 152.02m));

            Assert.IsTrue(matchesTrue.Matches(true));
            Assert.IsFalse(matchesTrue.Matches(false));
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
