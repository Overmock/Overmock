using System;

namespace Kimono.Core
{
    public interface IDelegateFactory
    {
        TDelegate CreateDelegate<TDelegate>(MethodMetadata metadata, IInvocation invocation) where TDelegate : Delegate;
    }
}