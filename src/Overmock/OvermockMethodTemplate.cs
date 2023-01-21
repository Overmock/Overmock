using Overmock.Runtime;
using System.Reflection;

namespace Overmock
{
    /// <summary>
    /// Do not use. Used for testing.
    /// </summary>
    public class OvermockMethodTemplate
    {
        private OvermockContext? _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void InitializeOvermock(OvermockContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="OvermockException"></exception>
        public string TestMethod(string name)
        {
            var handle = _context.Get((MethodInfo)MethodBase.GetCurrentMethod()!);
            var result = handle.Handle(name);

            if (result.Result != null)
            {
                return (string)result.Result;
            }

            throw new OvermockException("oops, didn't handle this method call.");
        }
    }
}