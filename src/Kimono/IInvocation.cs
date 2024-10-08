﻿using System;
using System.Reflection;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInvocation
    {
        /// <summary>
        /// The name of the member being invoked.
        /// </summary>
        public string MemberName => Method?.Name ?? string.Empty;

        /// <summary>
        /// 
        /// </summary>
        Type[]? GenericParameters { get; }

        /// <summary>
        /// 
        /// </summary>
        ParameterInfo[]? ParameterTypes { get; }

        /// <summary>
        /// 
        /// </summary>
        object?[]? Parameters { get; }
        
        /// <summary>
        /// 
        /// </summary>
        MethodInfo? Method { get; }

        /// <summary>
        /// 
        /// </summary>
        object? ReturnValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool IsProperty { get; }

        /// <summary>
        /// 
        /// </summary>
        bool TargetInvoked { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setReturnValue"></param>
        void Invoke(bool setReturnValue = true);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        T GetParameter<T>(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        T GetParameter<T>(int index);
    }
}