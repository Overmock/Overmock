using Overmock.Compilation.Roslyn;

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

			public const string NamespaceAndClassAreRequiredToBuild = "NamespaceDeclaration and ClassDeclaration are required to build a CompilationUnitSyntax";
			public const string NumberOfParameterMismatch = "The number of parameters supplied doesn't not match the method's signature.";
		}
	}

	internal static class Throw
	{
		public static class If
		{
			public static Type DeclaringTypeNull(Type? declaringType, string childName, Exception? innerException = default)
			{
				if (declaringType == null)
				{
					throw new ArgumentNullException($"Member: '{childName}' must have a declaring type.", innerException);
				}

				return declaringType;
			}

			internal static void BuildComponentsAreNull(RoslynAssemblyGenerationContext context)
			{
				if (context.NamespaceDeclaration == null || context.ClassDeclaration == null)
				{
					throw new InvalidOperationException(Ex.Message.NamespaceAndClassAreRequiredToBuild);
				}
			}
		}
	}
}