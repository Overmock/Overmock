using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kimono.Core
{
    public sealed class MethodMetadata
    {
        private readonly MethodInfo _targetMethod;

        private ParameterInfo[]? _parameters;
        private Type[]? _parameterTypes;
        private List<ParameterInfo>? _inputs;
        private List<ParameterInfo>? _outs;
        private List<Type>? _generics;

        private MethodMetadata(MethodInfo targetMethod)
        {
            _targetMethod = targetMethod;
        }

        public MethodInfo TargetMethod => _targetMethod;

        public ParameterInfo[] Parameters => _parameters ??= _targetMethod.GetParameters();

        public Type[] ParameterTypes => _parameterTypes ??= Parameters.Select(p => p.ParameterType).ToArray();

        public IReadOnlyList<ParameterInfo> InputParameters =>
            _inputs ??= GetParameters();

        public IReadOnlyList<ParameterInfo> OutParameters =>
            _outs ??= GetParameters(false);

        public Type ReturnType => _targetMethod.ReturnType;

        public IReadOnlyList<Type> GenericParameters => _generics ??= CreateList(
            _targetMethod.ContainsGenericParameters
                ? _targetMethod.GetGenericArguments()
                : Array.Empty<Type>()
        );

        private List<ParameterInfo> GetParameters(bool inParameters = true)
        {
            return CreateList(
                Parameters.Where(
                    p => inParameters && p.IsIn || !inParameters && p.IsOut
                ).ToArray()
            );
        }

        public static MethodMetadata FromMethod(MethodInfo targetMethod)
        {
            return new MethodMetadata(targetMethod);
        }

        private List<T> CreateList<T>(T[] array)
        {
            return new List<T>(array);
        }
    }

    public interface IParameterCollection
    {

    }

    public sealed class ParameterCollection : IParameterCollection
    {
        public ParameterCollection()
        {

        }
    }
}