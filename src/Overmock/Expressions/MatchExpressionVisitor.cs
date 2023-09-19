using Overmocked.Matchable;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace Overmocked.Expressions
{
    internal class MatchExpressionVisitor : ExpressionVisitor, IMatchExpressionVisitor
    {
        private readonly List<IMatch> _matches = new List<IMatch>();

        public IMatch[] VisitMethod(MethodCallExpression methodExpression)
        {
            var method = methodExpression.Method;
            var arguments = methodExpression.Arguments;
            //var parameters = method.GetParameters();

            for (int i = 0; i < arguments.Count; i++)
            {
                Expression? expression = arguments[i];

                if (expression is UnaryExpression unary)
                {
                    expression = unary.Operand;

                    if (unary.Operand is MethodCallExpression methodCall)
                    {
                        VisitMethodCall(methodCall);
                    }
                    else if (expression is MemberExpression member)
                    {
                        var field = (System.Reflection.FieldInfo)member.Member;

                        if (member.Expression is ConstantExpression constant)
                        {
                            if (constant.Value is IMatch match)
                            {
                                _matches.Add(match);
                            }
                            else
                            {
                                _matches.Add((IMatch)field.GetValue(constant.Value)!);
                            }
                        }
                        else
                        {
                            _matches.Add(new Any<object>());
                        }
                    }
                    else
                    {
                        _matches.Add(new Any<object>());
                    }
                }
                else
                {
                    _matches.Add(new Any<object>());
                }
            }
            
            return _matches.ToArray();
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (Types.Match.IsAssignableFrom(node.Type))
            {
                var arguments = node.Arguments;
                var parameters = new object?[arguments.Count];

                for (int i = 0; i < parameters.Length; i++)
                {
                    Expression? expression = arguments[i];

                    var visitedExpression = Visit(expression);

                    if (visitedExpression is ConstantExpression constant)
                    {
                        parameters[i] = constant.Value;
                    }
                    else if (expression is MemberExpression member)
                    {
                        var field = (System.Reflection.FieldInfo)member.Member;

                        if (member.Expression is ConstantExpression parentObject)
                        {
                            parameters[i] = field.GetValue(parentObject.Value)!;
                        }
                    }
                }

                _matches.Add((IMatch)node.Method.Invoke(null, parameters)!);
            }

            return base.VisitMethodCall(node);
        }

        private static class Types
        {
            internal static readonly Type Match = typeof(IMatch);
        }
    }
}