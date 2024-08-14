using Kimono.Tests.Proxies;
using Microsoft.Extensions.Logging;

namespace Kimono.Tests.Logging
{
    [TestClass]
    public class MsLoggingTests
    {
        //[TestMethod]
        //public void ClrRuntimeLoggerTest()
        //{
        //    var interceptor = new TestInterceptor<ILogger<MsLoggingTests>>();

        //    var logger = //Intercept.WithCallback<ILogger<MsLoggingTests>>(c => { });
        //        ProxyFactory.Create().CreateInterfaceProxy(interceptor);

        //    try
        //    {
        //        logger.LogError(new Exception(), string.Empty);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        [TestMethod]
        public void TargetedParamsMethodTest()
        {
            var interceptor = new TestInterceptor<IParams>();

            var logger = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

            logger.Params("test", 1, new Model());
        }

        [TestMethod]
        public void ParamsMethodTest()
        {
            var interceptor = new TestCallbackInterceptor<IParams>(new ParamsClass(), c => { c.Invoke(); });

            var target = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

            target.Params("test", 1, new Model());
        }

        [TestMethod]
        public void LogMethodTest()
        {
            var interceptor = new TestCallbackInterceptor<IParams>(new ParamsClass(), c => { c.Invoke(); });

            var target = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

            target.Log<IReadOnlyList<string>>(LogLevel.Error, new EventId(1), new List<string>(), null, (l, e) => string.Empty);
        }

        [TestMethod]
        public void ExtensionMethodTest()
        {
            var interceptor = new TestCallbackInterceptor<IParams>(new ParamsClass(), c => { c.Invoke(); });

            var target = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

            target.Test(new Exception(), string.Empty, "hello", "world");
        }

        private sealed class TestInterceptor<T> : Interceptor<T> where T : class
        {
            protected override void HandleInvocation(IInvocation invocation)
            {
            }
        }
    }

    public interface IParams
    {
        string Params(params object[] args)
        {
            return "params";
        }
        void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
        }
    }

    public class ParamsClass : IParams
    {
    }

    public static class ExtedoMatic
    {
        internal static void Test(this IParams iparams, Exception ex, string message, params object[] args)
        {
            iparams.Log(LogLevel.Error, 0, new Model(), null, (_, _) => message);
        }
    }
}
