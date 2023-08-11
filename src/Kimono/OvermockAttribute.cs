using System.Runtime.CompilerServices;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class KimonoAttribute : CustomConstantAttribute
    {
        private readonly int _methodId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodId"></param>
        public KimonoAttribute(int methodId)
        {
            _methodId = methodId;
        }

        /// <summary>
        /// 
        /// </summary>
        public override object? Value => _methodId;
    }
}