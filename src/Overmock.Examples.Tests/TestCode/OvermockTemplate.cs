using Overmock.Runtime.Proxies;

namespace Overmock.Examples.Tests.TestCode
{
    /// <summary>
    /// Do not use. Used for testing.
    /// </summary>
    public class OvermockTemplate
    {
#pragma warning disable IDE1006 // Naming Styles
        private ProxyOverrideContext? ___context;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void InitializeOvermockContext(ProxyOverrideContext context)
        {
            ___context = context;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        ///// <exception cref="OvermockException"></exception>
        //[Overmock("71a8440c-ba80-472c-bc31-a3736c3e5b4c")]
        //public object TestMethod(string name)
        //{
        //	var handle = ___context.Get((MethodInfo)MethodBase.GetCurrentMethod()!);
        //	var result = handle.Handle(name);
        //	return (Type)result.Result;
        //}
    }
}