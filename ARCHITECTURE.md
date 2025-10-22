# Overmock Architecture

## Overview

Overmock is a .NET mocking framework built on top of **Kimono**, a high-performance dynamic proxy generation library. The architecture is designed in two distinct layers, each with clear responsibilities.

```
┌─────────────────────────────────────────┐
│         Overmock (Mocking Layer)        │
│  ┌─────────────────────────────────┐   │
│  │  Fluent API for Mocking         │   │
│  │  - Mock<T>()                    │   │
│  │  - .ToReturn(), .ToThrow()      │   │
│  │  - .Verify()                    │   │
│  └─────────────────────────────────┘   │
└─────────────────────────────────────────┘
                  ▼
┌─────────────────────────────────────────┐
│       Kimono (Proxy Generation)         │
│  ┌─────────────────────────────────┐   │
│  │  Dynamic IL Generation          │   │
│  │  - ProxyFactory                 │   │
│  │  - Reflection.Emit              │   │
│  │  - Interceptor Pattern          │   │
│  └─────────────────────────────────┘   │
└─────────────────────────────────────────┘
                  ▼
┌─────────────────────────────────────────┐
│         .NET Runtime (CLR)              │
└─────────────────────────────────────────┘
```

## Layer 1: Kimono - Dynamic Proxy Generation

### Purpose
Kimono is responsible for creating dynamic implementations of interfaces at runtime using `System.Reflection.Emit`. It provides the low-level infrastructure for proxying without any mocking-specific logic.

### Key Components

#### 1. ProxyFactory
**Location:** `src/Kimono/ProxyFactory.cs`

The main entry point for creating proxies. It orchestrates the entire proxy generation process:

```csharp
var factory = ProxyFactory.Create();
var proxy = factory.CreateInterfaceProxy<IMyInterface>(interceptor);
```

**Responsibilities:**
- Creates TypeBuilder for the proxy type
- Implements all interface methods and properties
- Manages proxy caching
- Generates constructors

**Process Flow:**
1. Check cache for existing proxy type
2. If not cached:
   - Create TypeBuilder inheriting from ProxyBase
   - Add interface implementation
   - Generate methods via CreateMethods()
   - Generate properties via CreateProperties()
   - Create the type with CreateType()
3. Return proxy instance

#### 2. Method Generation (CreateMethods)
**Location:** `src/Kimono/ProxyFactory.cs:327`

For each method in the interface:
1. Creates a MethodBuilder with matching signature
2. **For generic methods:** Calls DefineGenericParameters to create new generic types
3. Emits IL via DelegateFactory.EmitProxyMethod
4. Optionally creates delegate invokers for performance

**Critical Detail - Generic Methods:**
When a method has generic parameters (like `ILogger<T>.Log<TState>`), we must:
- Call `DefineGenericParameters` to create new generic parameter types on the MethodBuilder
- Use these **new types** (not the original method's types) in IL generation
- Pass them to `EmitProxyMethod` via the `genericParameterTypes` parameter

**Why?** The `ldtoken` IL instruction can only reference types that are valid in the current MethodBuilder's context. Using the original method's generic types causes a `BadImageException`.

#### 3. IL Generation (EmitProxyMethod)
**Location:** `src/Kimono/DelegateFactory.cs:200`

Generates IL that intercepts method calls:

```
┌──────────────────────────────────────┐
│  Generated IL for Method<T>(arg)     │
├──────────────────────────────────────┤
│  1. Load generic type T              │
│     - ldtoken T                      │
│     - call GetTypeFromHandle         │
│     - store in local                 │
│                                      │
│  2. Package arguments                │
│     - Create object[] array          │
│     - Box value types                │
│     - Store each arg in array        │
│                                      │
│  3. Call interceptor                 │
│     - ldarg.0 (this)                 │
│     - ldc.i4 methodId                │
│     - load Type[] array              │
│     - load object[] array            │
│     - callvirt HandleMethodCall      │
│                                      │
│  4. Handle return value              │
│     - Unbox/cast to return type      │
│     - Return                         │
└──────────────────────────────────────┘
```

**Key IL Methods:**
- `EmitGenericParameters`: Loads generic types into locals using ldtoken
- `EmitGenericLocalFieldTypes`: Creates Type[] array from locals
- Boxing/unboxing logic for value types

#### 4. Interceptor Pattern
**Location:** `src/Kimono/IInterceptor.cs`, `src/Kimono/Interceptor.cs`

The core extension point. All method calls are routed through:

```csharp
public interface IInterceptor
{
    object? HandleInvocation(int methodId, Type[] genericParameters, object[] parameters);
}
```

**Base Classes:**
- `InterceptorBase`: Low-level interceptor with method ID handling
- `Interceptor<T>`: High-level interceptor with IInvocation API

#### 5. ProxyBase
**Location:** `src/Kimono/ProxyBase.cs`

All generated proxy types inherit from ProxyBase:

```csharp
public abstract class ProxyBase<TInterceptor> : IProxy where TInterceptor : IInterceptor
{
    protected TInterceptor _interceptor;

    protected object? HandleMethodCall(int methodId, Type[] genericParameters, object[] parameters)
    {
        return _interceptor.HandleInvocation(methodId, genericParameters, parameters);
    }
}
```

### Performance Optimizations

1. **Proxy Caching**: Generated types are cached by interface type
2. **Delegate Invokers**: For frequently called methods, create optimized delegates
3. **IL Generation**: Direct IL emit is faster than pure reflection

## Layer 2: Overmock - Mocking Framework

### Purpose
Overmock builds on Kimono to provide a fluent, type-safe mocking API similar to Moq or NSubstitute.

### Key Components

#### 1. Overmock<T>
**Location:** `src/Overmock/Overmock.cs`

The main mock object that wraps a Kimono proxy:

```csharp
var mock = Overmock.Mock<IRepository>();
mock.Mock(m => m.GetById(42))
    .ToReturn(new User { Id = 42 });
```

**Responsibilities:**
- Creates Kimono proxy with OvermockInterceptor
- Provides fluent API for setup
- Manages method call expectations
- Handles verification

#### 2. OvermockInterceptor
**Location:** `src/Overmock/OvermockInterceptor.cs`

Implements IInterceptor to provide mocking behavior:

```csharp
public override void HandleInvocation(IInvocation invocation)
{
    // 1. Check if this method call has a setup (ToReturn, ToThrow, etc.)
    if (TryGetOverride(invocation, out var @override))
    {
        // Execute the setup behavior
        invocation.ReturnValue = @override.Execute(invocation);
    }
    else
    {
        // Return default value for type
        invocation.ReturnValue = GetDefaultValue(invocation.Method.ReturnType);
    }
}
```

#### 3. Method Call Setup
**Location:** `src/Overmock/Mocking/Internal/MethodCall.cs`

When you call `.Mock(m => m.Method())`, Overmock:
1. Invokes the method on the proxy
2. Captures the invocation via the interceptor
3. Creates a MethodCall object
4. Returns IMethodCall<TReturn> for fluent setup

```csharp
public interface IMethodCall<TReturn>
{
    IMethodCall<TReturn> ToReturn(TReturn value);
    IMethodCall<TReturn> ToReturn(Func<TReturn> func);
    IMethodCall<TReturn> ToThrow<TException>() where TException : Exception, new();
    IVerifiable Verifiable();
}
```

#### 4. Parameter Matching
**Location:** `src/Overmock/Its.cs`, `src/Overmock/Matchable/`

Supports flexible parameter matching:

```csharp
mock.Mock(m => m.GetById(Its.Any<int>()))
    .ToReturn(new User());

mock.Mock(m => m.Save(Its.This(user)))
    .ToReturn(true);
```

**Matchers:**
- `Its.Any<T>()`: Matches any value of type T
- `Its.This(value)`: Matches specific value by equality

#### 5. Verification
**Location:** `src/Overmock/Mocking/Internal/Verifiable.cs`

Verifies method calls occurred:

```csharp
var call = mock.Mock(m => m.Save(user))
    .Verifiable();

// Later...
call.Verify(Times.Once());
```

## Data Flow

### Setup Phase

```
User Code                Overmock              Kimono Proxy         Interceptor
    │                       │                       │                    │
    │  Mock<IRepo>()        │                       │                    │
    ├──────────────────────>│                       │                    │
    │                       │  CreateProxy()        │                    │
    │                       ├──────────────────────>│                    │
    │                       │                       │                    │
    │                       │  Proxy w/Interceptor  │                    │
    │<──────────────────────┤<──────────────────────┤                    │
    │                       │                       │                    │
    │  .Mock(m => m.Get(42))│                       │                    │
    ├──────────────────────>│                       │                    │
    │                       │  m.Get(42)            │                    │
    │                       ├──────────────────────>│                    │
    │                       │                       │  HandleInvocation()│
    │                       │                       ├───────────────────>│
    │                       │                       │    (Capture call)  │
    │                       │  MethodCall object    │                    │
    │<──────────────────────┤                       │                    │
    │                       │                       │                    │
    │  .ToReturn(result)    │                       │                    │
    ├──────────────────────>│                       │                    │
    │                       │  (Store override)     │                    │
```

### Execution Phase

```
User Code                Overmock              Kimono Proxy         Interceptor
    │                       │                       │                    │
    │  proxy.Get(42)        │                       │                    │
    ├──────────────────────────────────────────────>│                    │
    │                       │                       │  HandleInvocation()│
    │                       │                       ├───────────────────>│
    │                       │                       │                    │
    │                       │  Check override       │                    │
    │                       │<──────────────────────┼────────────────────┤
    │                       │  Execute override     │                    │
    │                       │───────────────────────┼───────────────────>│
    │                       │  Return result        │                    │
    │                       │<──────────────────────┼────────────────────┤
    │  result               │                       │                    │
    │<──────────────────────────────────────────────┤                    │
```

## Generic Method Handling

### The Challenge

Generic methods like `ILogger<T>.Log<TState>` present a unique challenge:

```csharp
void Log<TState>(LogLevel level, EventId eventId, TState state,
                 Exception exception, Func<TState, Exception, string> formatter)
```

When creating a proxy, we need to:
1. Create a new method on the proxy with its own generic parameters
2. Generate IL that references these parameters
3. Forward the call to the interceptor with type information

### The Solution

**Step 1: Define Generic Parameters (ProxyFactory:239)**
```csharp
private static Type[] DefineGenericParameters(MethodMetadata metadata, MethodBuilder methodBuilder)
{
    // Create new generic parameters: TState
    var builders = methodBuilder.DefineGenericParameters("TState");

    // Copy constraints from original
    builders[0].SetGenericParameterAttributes(...);

    // Return the NEW types (critical!)
    return builders.Cast<Type>().ToArray();
}
```

**Step 2: Emit IL with New Types (DelegateFactory:412)**
```csharp
private static LocalBuilder[] EmitGenericParameters(IEmitter emitter,
    MethodMetadata metadata, Type[]? genericParameterTypes)
{
    // Use genericParameterTypes (from MethodBuilder) NOT metadata.GenericParameters
    var arguments = genericParameterTypes ?? metadata.GenericParameters;

    // Emit: ldtoken TState (new type, not original)
    emitter.IlGenerator.Emit(OpCodes.Ldtoken, arguments[i]);
    emitter.IlGenerator.Emit(OpCodes.Call, Methods.GetTypeFromHandle);
}
```

**Why This Matters:**

The `ldtoken` instruction requires a type that exists in the current IL context. If we use the generic parameter from the original `ILogger<T>.Log<TState>`, we get:

```
BadImageException: Token <some-token> is not valid in the scope of module Proxy-ILogger.
```

By using the generic parameters from the MethodBuilder (the ones we just created), the ldtoken instruction references types that are valid in the proxy method's scope.

## Extension Points

### For Users

1. **Custom Interceptors**: Implement `IInterceptor<T>` for custom behavior
2. **Delegate Factories**: Implement `IDelegateFactory` for custom IL generation strategies
3. **Proxy Caching**: Implement `IProxyGeneratorCache` for custom caching

### For Contributors

1. **IL Generation**: Modify `DelegateFactory.EmitProxyMethod` for new IL patterns
2. **Method Metadata**: Extend `MethodMetadata` for additional method information
3. **Mocking Features**: Add new setup methods to `Overmock<T>`

## Testing Strategy

### Kimono Tests
**Location:** `tests/Kimono.Tests/`

- **InterfaceProxyTests**: Basic proxy generation
- **NewDynamicMethodGenerationTests**: Method generation
- **MsLoggingTests**: Generic method handling (ILogger)
- **GenericMethodEdgeCasesTests**: Comprehensive generic method scenarios

### Overmock Tests
**Location:** `tests/Overmock.Tests/`

- **MethodWithNoParams.Tests**: Basic mocking
- **MethodGenericNoParameters.Tests**: Generic method mocking
- **PropertyGet.Tests**: Property mocking
- **AsyncMethodTests**: Async/await support

### Test Philosophy

1. **Kimono tests** focus on proxy generation correctness
2. **Overmock tests** focus on mocking behavior and API usability
3. **Edge case tests** ensure complex scenarios (like ILogger) work

## Performance Characteristics

### Proxy Generation
- **First Call**: ~1-5ms (includes IL generation and type creation)
- **Cached Calls**: ~0.001ms (returns cached type)

### Method Invocation
- **With Delegate Invokers**: ~0.01ms
- **Without Invokers**: ~0.1ms (uses reflection)
- **Compared to Direct Call**: ~10-100x slower (acceptable for testing)

### Memory
- **Per Proxy Type**: ~50-200 KB (depending on interface size)
- **Per Proxy Instance**: ~200 bytes + interceptor size

## Known Limitations

1. **Interface Only**: Cannot proxy classes (by design)
2. **No Partial Mocking**: All methods must be intercepted
3. **No Mock Inheritance**: Cannot inherit mock setups
4. **Generic Type Constraints**: Some complex constraints may not be supported

## Future Considerations

1. **Source Generators**: Could replace some IL emit with compile-time generation (C# 9+)
2. **Record Types**: Better support for C# 9+ records
3. **async/await**: Optimize async method handling
4. **Performance**: Reduce allocation in hot paths

## Conclusion

The two-layer architecture provides:
- **Separation of concerns**: Kimono handles proxying, Overmock handles mocking
- **Reusability**: Kimono can be used standalone for other purposes
- **Performance**: Direct IL generation beats reflection-heavy approaches
- **Maintainability**: Clear boundaries between layers

The generic method fix demonstrates the importance of understanding IL generation deeply - a small mistake (using the wrong Type reference) can cause runtime failures that are difficult to diagnose.
