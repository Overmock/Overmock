using Overmocked.Mocking;
using System;

namespace Overmocked
{
    /// <summary>
    /// Class Ex.
    /// </summary>
    internal static class Ex
    {
        /// <summary>
        /// Class Message.
        /// </summary>
        public static class Message
        {
            /// <summary>
            /// The namespace and class are required to build
            /// </summary>
            public const string NamespaceAndClassAreRequiredToBuild = "NamespaceDeclaration and ClassDeclaration are required to build a CompilationUnitSyntax";
            
            /// <summary>
            /// The number of parameter mismatch
            /// </summary>
            public const string NumberOfParameterMismatch = "The number of parameters supplied doesn't not match the method's signature.";

            /// <summary>
            /// Generals the specified verifiable.
            /// </summary>
            /// <param name="verifiable">The verifiable.</param>
            /// <param name="message">The message.</param>
            /// <param name="innerException">The inner exception.</param>
            /// <returns>System.String.</returns>
            public static string General(IVerifiable verifiable, string? message = null, Exception? innerException = default)
            {
                return string.IsNullOrWhiteSpace(message)
                    ? $"{verifiable.GetType()} failed with generic message: {innerException?.Message ?? "innerException is null"}"
                    : message;
            }

            /// <summary>
            /// Nots the type of an interface.
            /// </summary>
            /// <param name="target">The target.</param>
            /// <returns>System.String.</returns>
            public static string NotAnInterfaceType(IOvermock target)
            {
                return $"Type is not an interface: {target.GetTarget()?.GetType().FullName}";
            }
        }
    }

    /// <summary>
    /// Class Throw.
    /// </summary>
    internal static class Throw
    {
        /// <summary>
        /// Class If.
        /// </summary>
        public static class If
        {
            /// <summary>
            /// Declarings the type null.
            /// </summary>
            /// <param name="declaringType">Type of the declaring.</param>
            /// <param name="childName">Name of the child.</param>
            /// <param name="innerException">The inner exception.</param>
            /// <returns>Type.</returns>
            /// <exception cref="System.ArgumentNullException">Member: '{childName}' must have a declaring type.</exception>
            public static Type DeclaringTypeNull(Type? declaringType, string childName, Exception? innerException = default)
            {
                if (declaringType == null)
                {
                    throw new ArgumentNullException($"Member: '{childName}' must have a declaring type.", innerException);
                }

                return declaringType;
            }
        }
    }
}