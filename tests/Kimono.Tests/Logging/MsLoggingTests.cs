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
        //    var logger = Intercept.WithCallback<ILogger<MsLoggingTests>>(c => { });

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
            var target = Intercept.WithCallback<IParams>(c => { });

            target.Params("test", 1, new Model());
        }

        [TestMethod]
        public void ParamsMethodTest()
        {
            var target = Intercept.WithCallback<IParams>(new ParamsClass(), c => { c.Invoke(); });

            target.Params("test", 1, new Model());
        }

        //[TestMethod]
        //public void LogMethodTest()
        //{
        //    var target = Intercept.WithCallback<IParams>(new ParamsClass(), c => { c.Invoke(); });

        //    target.Log<IReadOnlyList<string>>(LogLevel.Error, new EventId(1), new List<string>(), null, (l, e) => string.Empty);
        //}

        //[TestMethod]
        //public void ExtensionMethodTest()
        //{
        //    var target = Intercept.WithCallback<IParams>(new ParamsClass(), c => { c.Invoke(); });

        //    target.Test(new Exception(), string.Empty, "hello", "world");
        //}
    }


    public interface IParams
    {
        string Params(params object[] args)
        {
            return "params";
        }
        void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
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
            iparams.Log(LogLevel.Error, 0, new Model(), null, (_, __) => message);
        }
    }
}
