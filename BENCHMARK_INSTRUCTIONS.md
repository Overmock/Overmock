# Benchmark Execution Instructions

## Quick Start

```bash
# Navigate to benchmark project
cd tests/Overmock.Benchmarks

# Run all benchmarks
dotnet run -c Release

# Or run specific benchmarks
dotnet run -c Release --filter "*NewProxyCreationBenchmark*"
dotnet run -c Release --filter "*InterceptorMethodCallBenchmark*"
```

---

## Prerequisites

- .NET 8.0 SDK installed
- Release build for accurate results
- Close other applications (reduce noise)
- Run multiple times for consistency

---

## Current Benchmarks

### 1. Proxy Creation Benchmark

**What it measures:**
- Time to create N proxy instances
- Memory allocated per proxy
- Comparison: Kimono vs Castle.Core vs DispatchProxy

**Command:**
```bash
dotnet run -c Release --filter "*NewProxyCreationBenchmark*"
```

**Expected output:**
```
| Method         | count   | Mean      | Allocated |
|--------------- |-------- |----------:|----------:|
| NewKimonoCore  | 1000    | ~1-2 ms   | ~200 KB   |
| NewCastle      | 1000    | ~2-5 ms   | ~300 KB   |
| NewDotnet      | 1000    | ~0.5-1 ms | ~150 KB   |
```

---

### 2. Method Invocation Benchmark

**What it measures:**
- Time to call a method on existing proxy
- Per-call overhead
- Comparison: Kimono vs Castle.Core vs DispatchProxy

**Command:**
```bash
dotnet run -c Release --filter "*InterceptorMethodCallBenchmark*"
```

**Expected output:**
```
| Method     | count | Mean     | Allocated |
|----------- |------ |---------:|----------:|
| KimonoCore | 1000  | ~0.01 ms | 0 B       |
| Castle     | 1000  | ~0.01 ms | 0 B       |
| Dotnet     | 1000  | ~0.1 ms  | 0 B       |
```

---

## Interpreting Results

### Good Results

✅ **Proxy Creation:** Within 2x of Castle.Core
✅ **Method Calls:** Within 1.5x of Castle.Core
✅ **Memory:** Competitive allocations
✅ **No regressions:** Same or better than v0.1.1

### Warning Signs

⚠️ **Proxy Creation:** > 3x slower than Castle
⚠️ **Method Calls:** > 2x slower than Castle
⚠️ **Memory:** Excessive allocations (> 500 KB for 1000 proxies)
⚠️ **Variance:** Results change significantly between runs

### Red Flags

❌ **Crashes** during benchmark
❌ **OutOfMemoryException**
❌ **Much slower** than DispatchProxy (shouldn't happen)

---

## Recommended Additional Benchmarks

### 1. Generic Method Benchmark

**Add this test:**

```csharp
// File: tests/Overmock.Benchmarks/GenericMethodBenchmark.cs
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class GenericMethodBenchmark
{
    private readonly IBenchmarkGeneric _kimono;
    private readonly IBenchmarkGeneric _castle;

    [GlobalSetup]
    public void Setup()
    {
        // Setup Kimono proxy
        var interceptor = new Interceptor<IBenchmarkGeneric>();
        _kimono = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

        // Setup Castle proxy
        var generator = new ProxyGenerator();
        _castle = generator.CreateInterfaceProxyWithoutTarget<IBenchmarkGeneric>(
            new CastleInterceptor());
    }

    [Benchmark]
    [Arguments(1_000)]
    public void Kimono_GenericMethod(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _kimono.GenericMethod<string>("test");
        }
    }

    [Benchmark]
    [Arguments(1_000)]
    public void Castle_GenericMethod(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _castle.GenericMethod<string>("test");
        }
    }
}

public interface IBenchmarkGeneric
{
    void GenericMethod<T>(T value);
    TResult GenericMethodWithReturn<T, TResult>(T input);
}
```

**Why:** Tests the ILogger fix

---

### 2. ILogger Benchmark

**Add this test:**

```csharp
using Microsoft.Extensions.Logging;
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class ILoggerBenchmark
{
    private readonly ILogger<ILoggerBenchmark> _kimono;
    private readonly ILogger<ILoggerBenchmark> _moq;

    [GlobalSetup]
    public void Setup()
    {
        // Kimono
        var interceptor = new Interceptor<ILogger<ILoggerBenchmark>>();
        _kimono = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

        // Moq (if it works)
        // var moqMock = new Mock<ILogger<ILoggerBenchmark>>();
        // _moq = moqMock.Object;
    }

    [Benchmark]
    [Arguments(1_000)]
    public void Kimono_ILogger(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _kimono.Log(LogLevel.Information, new EventId(1), "test", null,
                (state, ex) => state);
        }
    }
}
```

**Why:** Demonstrates the v0.1.2 fix value

---

### 3. Property Access Benchmark

```csharp
[Benchmark]
public void PropertyGet(int count)
{
    for (int i = 0; i < count; i++)
    {
        var value = _proxy.SomeProperty;
    }
}

[Benchmark]
public void PropertySet(int count)
{
    for (int i = 0; i < count; i++)
    {
        _proxy.SomeProperty = i;
    }
}
```

**Why:** Properties are common in mocking

---

## Publishing Results

### 1. Save Results

BenchmarkDotNet automatically saves to:
```
tests/Overmock.Benchmarks/BenchmarkDotNet.Artifacts/results/
```

### 2. Add to README

```markdown
## Performance

Benchmarks run on .NET 8.0, Windows/Linux, [CPU details]

### Proxy Creation (1,000 instances)

| Framework    | Mean   | Allocated |
|-------------|--------|-----------|
| Kimono      | X.XX ms| XXX KB    |
| Castle.Core | X.XX ms| XXX KB    |
| DispatchProxy | X.XX ms | XXX KB |

### Method Invocation (1,000 calls)

| Framework    | Mean   | Allocated |
|-------------|--------|-----------|
| Kimono      | X.XX ms| X B       |
| Castle.Core | X.XX ms| X B       |
| DispatchProxy | X.XX ms | X B    |

[See full benchmark results](./BENCHMARK_RESULTS.md)
```

### 3. Add to CHANGELOG

```markdown
## [0.1.2] - 2025-10-22

### Performance
- Proxy creation: X.XXms for 1,000 instances (within Xx% of Castle.Core)
- Method calls: X.XXms for 1,000 invocations (competitive with Castle.Core)
- Memory: XXX KB for 1,000 proxies
```

---

## Troubleshooting

### "dotnet command not found"

Install .NET SDK from https://dot.net

### Benchmarks take too long

Run with fewer iterations:
```bash
dotnet run -c Release -- --filter "*" --iterationCount 3
```

### Results are inconsistent

1. Close other applications
2. Run multiple times and average
3. Use Release build (not Debug)
4. Disable antivirus temporarily

### OutOfMemoryException

Reduce count arguments:
```csharp
[Arguments(1_000)]    // Instead of 1_000_000
```

---

## Next Steps After Running

1. ✅ Review results for any red flags
2. ✅ Compare to expectations (see PERFORMANCE_ANALYSIS.md)
3. ✅ Add results to README.md
4. ✅ Add results to CHANGELOG.md
5. ✅ Commit results to repository
6. ✅ Include in v0.1.2 release notes

---

## Advanced: CI Integration

### GitHub Actions

```yaml
name: Benchmarks

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  benchmark:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0.x
    - name: Run benchmarks
      run: |
        cd tests/Overmock.Benchmarks
        dotnet run -c Release -- --filter "*" --exporters json
    - name: Upload results
      uses: actions/upload-artifact@v3
      with:
        name: benchmark-results
        path: tests/Overmock.Benchmarks/BenchmarkDotNet.Artifacts/**/*
```

---

**Last Updated:** 2025-10-22
**Version:** 1.0
