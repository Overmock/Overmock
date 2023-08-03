# Overmock
![DOTNET Build](https://github.com/overmock/overmock/actions/workflows/dotnet.yml/badge.svg)

` C#
using System;
using System.Reflection;

public class C {
    public void M() {
    }
}
namespace Overmock
{
    public class OvermockMethodTemplate
    {
        private OvermockContext _context;

        public void InitializeOvermock(OvermockContext context)
        {
            _context = context;
        }

        public string TestMethod(string name)
        {
            var handle = _context.Get((MethodInfo)MethodBase.GetCurrentMethod()!);
            var result = handle.Handle(name);

            if (result.Result != null)
            {
                return (string)result.Result;
            }

            throw new Exception("oops, didn't handle this method call.");
        }
    }
    
    public class OvermockContext
    {
        public IHandler Get(MethodInfo method)
        {
            return null;
        }
    }
    
    public interface IHandler
    {
        IResult Handle(params object[] parameters);
    }
    
    public interface IResult
    {
        Object Result { get; }
    }
}
```
