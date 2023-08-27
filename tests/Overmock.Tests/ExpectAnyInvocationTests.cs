using Overmocked.Tests.Mocks.Methods;

namespace Overmocked.Tests
{
    [TestClass]
    public class ExpectAnyInvocationTests
    {
        [TestMethod]
        public void ExpectAnyInvocationOfTAllowsAnyInvocationOnAnyMemberOfANewOvermock()
        {
            var overmock = Overmock.AnyInvocation<IMethodsWithNoParameters>();

            for (int i = 0; i < 10; i++)
            {
                overmock.BoolMethodWithNoParams();
            }
        }

        [TestMethod]
        public void ExpectAnyInvocationOfTAllowsAnyInvocationOnAnyMemberOfAnExistingTarget()
        {
            var overmock = Overmock.AnyInvocation<IMethodsWithNoParameters>();


            for (int i = 0; i < 10; i++)
            {
                overmock.BoolMethodWithNoParams();
            }
        }

        [TestMethod]
        public void ExpectAnyInvocationOfTAllowsAnyInvocationOnAnyMemberOfAnExistingOvermock()
        {
            var overmock = Overmock.AnyInvocation<IMethodsWithNoParameters>();

            for (int i = 0; i < 10; i++)
            {
                overmock.BoolMethodWithNoParams();
            }
        }
    }
}
