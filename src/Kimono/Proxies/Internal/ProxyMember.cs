using Kimono.Proxies;
using System;
using System.Reflection;

namespace Kimono.Proxies.Internal
{
    /// <summary>
    /// Class ProxyMember.
    /// Implements the <see cref="IProxyMember" />
    /// </summary>
    /// <seealso cref="IProxyMember" />
    public sealed class ProxyMember : IProxyMember
    {
        /// <summary>
        /// The delegate
        /// </summary>
        private readonly Func<object?, object?[], object?> _delegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyMember"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        public ProxyMember(MethodInfo method) : this(method, method)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyMember"/> class.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="method">The method.</param>
        public ProxyMember(MemberInfo member, MethodInfo method)
        {
            _delegate = method.Invoke;

            Member = member;
            Method = method;
        }

        /// <summary>
        /// Gets the member.
        /// </summary>
        /// <value>The member.</value>
        public MemberInfo Member { get; }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <value>The method.</value>
        public MethodInfo Method { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => Member.Name;

        /// <summary>
        /// Creates the delegate.
        /// </summary>
        /// <returns>Func&lt;System.Object, System.Nullable&lt;System.Object&gt;[], System.Nullable&lt;System.Object&gt;&gt;.</returns>
        public Func<object?, object?[], object?> CreateDelegate()
        {
            return _delegate;
        }
    }
}