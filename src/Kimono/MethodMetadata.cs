using Kimono.Delegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MethodMetadata
    {
        private readonly MethodInfo _targetMethod;
        private ParameterInfo[]? _parameters;
        private Type[]? _parameterTypes;
        private List<ParameterInfo>? _inputs;
        private List<ParameterInfo>? _outs;
        private Type[]? _generics;
        private IDelegateInvoker? _invoker;

        private MethodMetadata(MethodInfo targetMethod, bool isProperty)
        {
            _targetMethod = targetMethod;
            IsProperty = isProperty;
        }

        /// <summary>
        /// 
        /// </summary>
        public MethodInfo TargetMethod => _targetMethod;

        /// <summary>
        /// 
        /// </summary>
        public ParameterInfo[] Parameters => _parameters ??= _targetMethod.GetParameters();
        
        /// <summary>
        /// 
        /// </summary>
        public Type[] ParameterTypes => _parameterTypes ??= Parameters.Select(p => p.ParameterType).ToArray();

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyList<ParameterInfo> InputParameters =>
            _inputs ??= GetParameters();

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyList<ParameterInfo> OutParameters =>
            _outs ??= GetParameters(false);

        /// <summary>
        /// 
        /// </summary>
        public Type ReturnType => _targetMethod.ReturnType;

        /// <summary>
        /// 
        /// </summary>
        public Type[] GenericParameters => _generics ??= CreateList(
            _targetMethod.ContainsGenericParameters
                ? _targetMethod.GetGenericArguments()
                : Array.Empty<Type>()
        ).ToArray();

        /// <summary>
        /// 
        /// </summary>
        public bool IsProperty { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetMethod"></param>
        /// <param name="isProperty"></param>
        /// <returns></returns>
        public static MethodMetadata FromMethodInfo(MethodInfo targetMethod, bool isProperty = false)
        {
            return new MethodMetadata(targetMethod, isProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invoker"></param>
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

        internal IDelegateInvoker? GetDelegateInvoker()
        {
            return _invoker;
        }

        private static List<T> CreateList<T>(T[] array)
        {
            return new List<T>(array);
        }
    }
}