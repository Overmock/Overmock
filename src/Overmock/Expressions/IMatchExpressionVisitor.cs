using System.Linq.Expressions;

namespace Overmocked.Expressions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMatchExpressionVisitor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IMatch[] VisitMethod(MethodCallExpression expression);
    }
}