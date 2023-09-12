using Kimono.Core.Internal;
using System;

namespace Kimono.Core
{
    public interface IInterceptor : IFluentInterface
	{
		object? HandleInvocation(int methodId, Type[] genericParameters, object[] parameters);
	}

    public interface IInterceptor<T> : IInterceptor
    {
        bool BuildInvoker { get; }
    }
}