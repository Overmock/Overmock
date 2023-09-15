using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kimono
{
    public class ProxyContext
    {
        private MethodMetadata[] _methods;

        public ProxyContext()
        {
            _methods = Array.Empty<MethodMetadata>();
        }

        public ref MethodMetadata GetMethod(int methodId)
        {
            //return _methods[methodId];
            ref var reference = ref MemoryMarshal.GetReference(_methods.AsSpan());
            return ref Unsafe.Add(ref reference, methodId);
        }

        public void SetMethods(MethodMetadata[] methods)
        {
            _methods = methods;
        }
    }
}