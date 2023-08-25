using Kimono.Proxies;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kimono.Internal
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
				if (propertyInfo.CanRead)
				{
					var methodId = context.GetNextMethodId();
					var property = new ProxyMember(propertyInfo, propertyInfo.GetGetMethod()!);
					var methodBuilder = CreateMethod(context, property.Method, methodId);

					context.ProxyContext.Add(new RuntimeContext(
						property,
						Enumerable.Empty<RuntimeParameter>())
					);
				}

				if (propertyInfo.CanWrite)
				{
					var methodId = context.GetNextMethodId();
					var property = new ProxyMember(propertyInfo, propertyInfo.GetSetMethod()!);
					var methodBuilder = CreateMethod(context, property.Method, methodId);

					context.ProxyContext.Add(new RuntimeContext(
						property,
						Enumerable.Empty<RuntimeParameter>())
					);
				}
			}
		}
	}
}
