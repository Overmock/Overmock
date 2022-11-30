namespace Overmock
{
    internal static class Ex
    {
        public static class Message
        {
            public static string General(IVerifiable verifiable, string? message = null, Exception? innerException = default)
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    return $"{verifiable.Type} failed with generic message: {innerException?.Message ?? "innerException is null"}";
                }

                return message;
            }
        }

        internal static class Throw
        {
            public static Type IfDeclaringTypeNull(Type? declaringType, string childName, Exception? innerException = default)
            {
                if (declaringType == null)
                {
                    throw new ArgumentNullException($"Member: '{childName}' must have a declaring type.", innerException);
                }

                return declaringType;
            }

            internal static void IfBuildComponentsAreNull(AssemblyGenerationContext context)
            {
                if (context.NamespaceDeclaration == null || context.ClassDeclaration == null)
                {
                    throw new InvalidOperationException("NamespaceDeclaration and ClassDeclaration are required to build a CompilationUnitSyntax");
                }
            }
        }
    }
}