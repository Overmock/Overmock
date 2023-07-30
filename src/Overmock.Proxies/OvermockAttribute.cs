using System.Runtime.CompilerServices;

namespace Overmock
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class OvermockAttribute : CustomConstantAttribute
    {
        private readonly string _methodId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodId"></param>
        public OvermockAttribute(string methodId)
        {
            _methodId = methodId;
        }

        /// <summary>
        /// 
        /// </summary>
        public override object? Value => _methodId;
    }
}