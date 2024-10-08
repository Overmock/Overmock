﻿using Kimono;

namespace Overmocked
{
    /// <summary>
    /// Class OvermockContext.
    /// </summary>
    public class OvermockContext
    {
        /// <summary>
        /// The invocation context
        /// </summary>
        private readonly IInvocation _invocationContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="OvermockContext"/> class.
        /// </summary>
        /// <param name="invocationContext">The invocation context.</param>
        public OvermockContext(IInvocation invocationContext)
        {
            _invocationContext = invocationContext;
        }

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns>T.</returns>
        public T Get<T>(string name)
        {
            return _invocationContext.GetParameter<T>(name);
        }

        /// <summary>
        /// Gets or sets the return value.
        /// </summary>
        /// <value>The return value.</value>
        public object? ReturnValue
        {
            get => _invocationContext.ReturnValue;
            set => _invocationContext.ReturnValue = value;
        }
    }
}
