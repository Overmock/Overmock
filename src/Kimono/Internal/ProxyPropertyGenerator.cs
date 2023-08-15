﻿using Kimono.Proxies;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Internal
{
	internal sealed class ProxyPropertyGenerator : ProxyMemberGenerator, IProxyPropertyGenerator
	{
		public void Generate(IProxyBuilderContext context, IEnumerable<PropertyInfo> properties)
		{
			ImplementProperties(context, properties);
		}

		/// <summary>
		/// Implements the properties.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="properties">The properties.</param>
		private static void ImplementProperties(IProxyBuilderContext context, IEnumerable<PropertyInfo> properties)
		{
			foreach (var propertyInfo in properties)
			{
				if (propertyInfo.CanRead)
				{
					var methodId = context.GetNextMethodId();
					var property = new ProxyMember(propertyInfo, propertyInfo.GetGetMethod()!);
					var methodBuilder = CreateMethod(context, property.Method, methodId);

					methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(
						_kimonoAttributeConstructor,
						new object[] { methodId }
					));

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

					methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(
						_kimonoAttributeConstructor,
						new object[] { methodId.ToString(Constants.CurrentCulture) }
					));

					context.ProxyContext.Add(new RuntimeContext(
						property,
						Enumerable.Empty<RuntimeParameter>())
					);
				}
			}
		}
	}
}