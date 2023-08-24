using Overmock.Tests.Mocks.Methods;

namespace Overmock.Tests
{
    [TestClass]
    public class ISetupMocksTests
    {
        [TestMethod]
        public void ISetupMocksTestsAllowsReturnsToBeMocked()
        {
            var overmock = Overmocked.ExpectAnyInvocation<IMethodsWithNoParameters>();

            for (int i = 0; i < 10; i++)
            {
                overmock.Target.BoolMethodWithNoParams();
            }
        }

        [TestMethod]
        public void ExpectAnyInvocationOfTAllowsAnyInvocationOnAnyMemberOfAnExistingTarget()
        {
            var overmock = Overmocked.ExpectAnyInvocation<IMethodsWithNoParameters>();

            for (int i = 0; i < 10; i++)
            {
                overmock.Target.BoolMethodWithNoParams();
            }
        }

        [TestMethod]
        public void ExpectAnyInvocationOfTAllowsAnyInvocationOnAnyMemberOfAnExistingOvermock()
        {
            var overmock = Overmocked.ExpectAnyInvocation<IMethodsWithNoParameters>();

            for (int i = 0; i < 10; i++)
            {
                overmock.Target.BoolMethodWithNoParams();
            }
        }
    }
}
