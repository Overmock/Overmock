using System.Reflection;

namespace Overmock.Compilation
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOvermockMethodBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        void BuildMethod(MethodInfo method);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constructor"></param>
        void BuildConstructor(ConstructorInfo? constructor = default);
    }
}