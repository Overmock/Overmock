using System.Runtime.CompilerServices;

namespace Overmock
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class OvermockAttribute : CustomConstantAttribute
    {
        private readonly int _methodId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodId"></param>
        public OvermockAttribute(int methodId)
        {
            _methodId = methodId;
        }

        /// <summary>
        /// 
        /// </summary>
        public override object? Value => _methodId;
    }
}