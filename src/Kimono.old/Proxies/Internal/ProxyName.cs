﻿using System;

namespace Kimono.Proxies.Internal
{
    internal static class ProxyName
    {
        private const char _typeNameDelimiterChar = '-';

        public static string GetName(IInterceptor interceptor)
        {
            var nameFormat = Constants.AssemblyAndTypeNameFormat;

            if (interceptor.TargetType.IsGenericTypeDefinition)
            {
                return nameFormat.ApplyFormat(
                    ReplaceInvalidCharacters(interceptor.TypeName)
                );
            }

            return nameFormat.ApplyFormat(interceptor.TypeName);
        }

        private static string ReplaceInvalidCharacters(string name)
        {
            return name.Replace(Type.Delimiter, _typeNameDelimiterChar);
        }
    }
}
