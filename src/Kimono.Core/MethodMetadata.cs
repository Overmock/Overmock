using Kimono.Delegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kimono.Core
{
    public sealed class MethodMetadata
    {
        private readonly MethodInfo _targetMethod;
        private readonly bool _isProperty;
        private ParameterInfo[]? _parameters;
        private Type[]? _parameterTypes;
        private List<ParameterInfo>? _inputs;
        private List<ParameterInfo>? _outs;
        private Type[]? _generics;
        private IDelegateInvoker? _invoker;

        private MethodMetadata(MethodInfo targetMethod, bool isProperty)
        {
            _targetMethod = targetMethod;
            _isProperty = isProperty;
        }

        public MethodInfo TargetMethod => _targetMethod;

        public ParameterInfo[] Parameters => _parameters ??= _targetMethod.GetParameters();

        public Type[] ParameterTypes => _parameterTypes ??= Parameters.Select(p => p.ParameterType).ToArray();

        public IReadOnlyList<ParameterInfo> InputParameters =>
            _inputs ??= GetParameters();

        public IReadOnlyList<ParameterInfo> OutParameters =>
            _outs ??= GetParameters(false);

        public Type ReturnType => _targetMethod.ReturnType;

        public Type[] GenericParameters => _generics ??= CreateList(
            _targetMethod.ContainsGenericParameters
                ? _targetMethod.GetGenericArguments()
                : Array.Empty<Type>()
        ).ToArray();

        public bool IsProperty => _isProperty;

        public static MethodMetadata FromMethodInfo(MethodInfo targetMethod, bool isProperty = false)
        {
            return new MethodMetadata(targetMethod, isProperty);
        }

        public void UseInvoker(IDelegateInvoker invoker)
        {
            _invoker = invoker;
        }

        private List<ParameterInfo> GetParameters(bool inParameters = true)
        {
            return CreateList(
                Parameters.Where(
                    p => inParameters && p.IsIn || !inParameters && p.IsOut
                ).ToArray()
            );
        }

        private List<T> CreateList<T>(T[] array)
        {
            return new List<T>(array);
        }

        internal IDelegateInvoker? GetInvoker()
        {
            return _invoker;
        }
    }
}