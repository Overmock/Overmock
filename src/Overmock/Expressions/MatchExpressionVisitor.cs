using Overmocked.Matchable;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Overmocked.Expressions
{
    internal sealed class MatchExpressionVisitor : ExpressionVisitor, IMatchExpressionVisitor
    {
        private readonly List<IMatch> _matches = new List<IMatch>();

        public IMatch[] VisitMethod(MethodCallExpression methodExpression)
        {
            // TODO: Need to re"visit" this. I don't like the current implementation 
            var method = methodExpression.Method;
            var arguments = methodExpression.Arguments;

            for (int i = 0; i < arguments.Count; i++)
            {
                Expression? expression = arguments[i];

                if (expression is UnaryExpression unary)
                {
                    expression = unary.Operand;

                    if (unary.Operand is MethodCallExpression methodCall)
                    {
                        _matches.Add(GetMethodMatch(methodCall));
                    }
                    else if (expression is MemberExpression member)
                    {
                        var field = (FieldInfo)member.Member;

                        if (member.Expression is ConstantExpression constant)
                        {
                            _matches.Add(GetConstantMatch(constant, field));
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
                else if (expression is ConstantExpression constant)
                {
                    _matches.Add(GetConstantMatch(constant, null));
                }
                else
                {
                    _matches.Add(new Any<object>());
                }
            }
            
            return _matches.ToArray();
        }

        private static IMatch GetMethodMatch(MethodCallExpression node)
        {
            if (Types.Match.IsAssignableFrom(node.Type))
            {
                var arguments = node.Arguments;
                var parameters = new object?[arguments.Count];

                for (int i = 0; i < parameters.Length; i++)
                {
                    Expression? expression = arguments[i];

                    if (expression is ConstantExpression constant)
                    {
                        parameters[i] = constant.Value;
                    }
                    else if (expression is MemberExpression member)
                    {
                        var field = (FieldInfo)member.Member;

                        if (member.Expression is ConstantExpression parentObject)
                        {
                            parameters[i] = field.GetValue(parentObject.Value)!;
                        }
                    }
                }

                return (IMatch)node.Method.Invoke(null, parameters)!;
            }

            return new Any<object>();
        }

        private static IMatch GetConstantMatch(ConstantExpression constant, FieldInfo? field = null)
        {
            if (constant.Value is IMatch match)
            {
                return match;
            }
            else if (!(field is null))
            {
                return (IMatch)field.GetValue(constant.Value)!;
            }

            return new This<object?>(constant.Value);
        }

        private static class Types
        {
            internal static readonly Type Match = typeof(IMatch);
        }
    }
}