using Overmock.Tests.Mocks.Methods;

namespace Overmock.Tests
{
    [TestClass]
    public class ExpectAnyInvocationTests
    {
        [TestMethod]
        public void ExpectAnyInvocationOfTAllowsAnyInvocationOnAnyMemberOfANewOvermock()
        {
            var overmock = Over.MockAnyInvocation<IMethodsWithNoParameters>();

            for (int i = 0; i < 10; i++)
            {
                overmock.Target.BoolMethodWithNoParams();
            }
        }

        [TestMethod]
        public void ExpectAnyInvocationOfTAllowsAnyInvocationOnAnyMemberOfAnExistingTarget()
        {
            var overmock = Over.MockAnyInvocation<IMethodsWithNoParameters>();


            for (int i = 0; i < 10; i++)
            {
                overmock.Target.BoolMethodWithNoParams();
            }
        }

        [TestMethod]
        public void ExpectAnyInvocationOfTAllowsAnyInvocationOnAnyMemberOfAnExistingOvermock()
        {
            var overmock = Over.MockAnyInvocation<IMethodsWithNoParameters>();

            for (int i = 0; i < 10; i++)
            {
                overmock.Target.BoolMethodWithNoParams();
            }
        }
    }
}
