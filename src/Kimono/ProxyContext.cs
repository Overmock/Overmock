﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxyContext
    {
        private readonly MethodMetadata[] _methods;

        /// <summary>
        /// 
        /// </summary>
        protected ProxyContext(MethodMetadata[] methods)
        {
            _methods = methods;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methods"></param>
        /// <returns></returns>
        public static ProxyContext Create(MethodMetadata[] methods)
        {
            return new ProxyContext(methods);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methods"></param>
        /// <returns></returns>
        public static ProxyContext Create<T>(MethodMetadata[] methods)
        {
            return new ProxyContext<T>(methods);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodId"></param>
        /// <returns></returns>
        public ref MethodMetadata GetMethod(int methodId)
        {
            ref var reference = ref MemoryMarshal.GetReference(_methods.AsSpan());
            return ref Unsafe.Add(ref reference, methodId);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProxyContext<T> : ProxyContext
    {
        /// <summary>
        /// 
        /// </summary>
        internal ProxyContext(MethodMetadata[] methods) : base(methods)
        {
        }
    }
}