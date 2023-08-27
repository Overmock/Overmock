using Kimono.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kimono.Proxies.Internal
{
    internal sealed class ProxyPropertyFactory : ProxyMemberFactory, IProxyPropertyFactory
    {
        public void Create(IProxyContextBuilder context, IEnumerable<PropertyInfo> properties)
        {
            CreateProperties(context, properties);
        }

        /// <summary>
        /// Implements the properties.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="properties">The properties.</param>
        private static void CreateProperties(IProxyContextBuilder context, IEnumerable<PropertyInfo> properties)
        {
            foreach (var propertyInfo in properties)
            {
                CreateProperty(context, propertyInfo);
            }
        }

        private static void CreateProperty(IProxyContextBuilder context, PropertyInfo propertyInfo)
        {
            if (propertyInfo.CanRead)
            {
                var methodId = context.GetNextMethodId();
                var property = new ProxyMember(propertyInfo, propertyInfo.GetGetMethod()!);
                var methodBuilder = CreateMethod(context, property.Method, Constants.EmptyRuntimParametersList, methodId);

                context.ProxyContext.Add(new RuntimeContext(
                    property,
                    Enumerable.Empty<RuntimeParameter>())
                );
            }

            if (propertyInfo.CanWrite)
            {
                var methodId = context.GetNextMethodId();
                var property = new ProxyMember(propertyInfo, propertyInfo.GetSetMethod()!);
                var methodBuilder = CreateMethod(context, property.Method,
                    new List<RuntimeParameter> { new RuntimeParameter("value", propertyInfo.PropertyType) },
                    methodId
                );

                context.ProxyContext.Add(new RuntimeContext(
                    property,
                    Enumerable.Empty<RuntimeParameter>())
                );
            }
        }
    }
}
