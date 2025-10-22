# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.1.2] - 2025-10-22

### Fixed
- **CRITICAL**: Fixed `BadImageException` when mocking `ILogger<T>` and other interfaces with generic methods
  - Root cause: IL generation was using generic parameter types from the original method instead of the MethodBuilder's newly created types
  - Fix: `DefineGenericParameters` now returns the MethodBuilder's generic parameter types and passes them to `EmitProxyMethod`
  - Impact: All interfaces with generic methods (like `ILogger<T>.Log<TState>`) now work correctly
  - Reference: `src/Kimono/ProxyFactory.cs:239`, `src/Kimono/DelegateFactory.cs:412`

### Added
- Comprehensive XML documentation for all Kimono public APIs
  - `IProxyFactory`: Complete interface documentation with usage examples
  - `IInterceptor`: Detailed explanation of the interceptor pattern
  - `IInvocation`: Documented invocation context and lifecycle
- Extensive inline documentation for IL generation code
  - `DelegateFactory.EmitProxyMethod`: Step-by-step IL generation explanation
  - `DelegateFactory.EmitGenericParameters`: Documented ldtoken usage and type context requirements
  - `DelegateFactory.EmitGenericLocalFieldTypes`: Explained Type[] array creation pattern
  - `ProxyFactory.DefineGenericParameters`: Documented generic parameter copying and constraints
  - `ProxyFactory.CreateMethods`: Explained proxy method generation pipeline
- Comprehensive test coverage for generic method edge cases in `GenericMethodEdgeCasesTests.cs`:
  - ILogger.Log with various logging levels
  - Multiple generic type parameters (2-3 parameters)
  - Generic constraints: class, struct, new(), interface, multiple combined
  - Generic methods with params arrays
  - Generic return types including Task<T>, IEnumerable<T>, nullable types
- Architecture documentation in `ARCHITECTURE.md`:
  - Two-layer architecture diagram and explanation
  - Detailed component descriptions with file locations
  - Data flow diagrams for setup and execution phases
  - Deep dive into generic method handling and the ILogger bug
  - IL generation patterns with actual opcode examples
  - Extension points for users and contributors
  - Performance characteristics and known limitations

### Removed
- **Technical Debt Cleanup**: Removed 92 obsolete files (~6,864 lines)
  - Deleted `src/Kimono.old/` directory containing legacy implementation
  - Deleted `src/Overmock.Compilation/IL/ProxyGeneratorUtils.cs` (1,319 lines of commented experimental code)
  - Benefit: Cleaner codebase, easier navigation, clear signal of active code

### Changed
- Uncommented `ClrRuntimeLoggerTest` in `MsLoggingTests.cs` - now passes with the ILogger fix

## [0.1.1] - Previous Release

### Added
- Parameter matching functionality
- Its.Any<T>() and Its.This(value) matchers

### Changed
- Finished Kimono restructure
- Trimmed unneeded classes

## [0.1.0] - Initial Release

### Added
- Initial Kimono proxy generation framework
- Initial Overmock mocking framework
- Basic mocking functionality with ToReturn(), ToThrow(), ToCall()
- Verification with Times matcher
- Support for methods and properties
- Multi-framework targeting: net8.0, net7.0, net6.0, netstandard2.1

---

## Version History Summary

- **0.1.2**: Critical bug fix for generic methods + comprehensive documentation
- **0.1.1**: Parameter matching and codebase cleanup
- **0.1.0**: Initial release with core mocking functionality

## Upgrade Guide

### From 0.1.1 to 0.1.2

**No breaking changes** - this is a backwards-compatible bug fix release.

Simply update your package reference:

```xml
<PackageReference Include="Overmock" Version="0.1.2" />
<PackageReference Include="Kimono" Version="0.1.2" />
```

**If you were affected by the ILogger bug**: Your previously failing code will now work:

```csharp
// This now works correctly (previously threw BadImageException)
var mock = Overmock.Mock<ILogger<MyClass>>();
mock.Mock(m => m.Log(
    Its.Any<LogLevel>(),
    Its.Any<EventId>(),
    Its.Any<object>(),
    Its.Any<Exception>(),
    Its.Any<Func<object, Exception, string>>()
)).Verifiable();
```

### From 0.1.0 to 0.1.2

Include all changes from 0.1.1:
- Update parameter matching to use `Its.Any<T>()` instead of custom matchers
- Review parameter matching in existing tests

## Breaking Changes

None in this release.

## Deprecations

None in this release.

## Security

### Security Measures in 0.1.2
- ✅ No unsafe code blocks
- ✅ Strong type validation in IL generation
- ✅ No code injection vectors in proxy generation
- ✅ All generated proxy types are sealed
- ✅ Proper input validation throughout
- ✅ Microsoft SourceLink enabled for debugging

### Reporting Security Issues
Please report security vulnerabilities to the repository maintainers privately before public disclosure.

## Links

- [Repository](https://github.com/Overmock/Overmock)
- [NuGet - Overmock](https://www.nuget.org/packages/Overmock/)
- [NuGet - Kimono](https://www.nuget.org/packages/Kimono/)
- [Documentation](https://github.com/Overmock/Overmock/blob/main/README.md)
- [Architecture](https://github.com/Overmock/Overmock/blob/main/ARCHITECTURE.md)

[Unreleased]: https://github.com/Overmock/Overmock/compare/v0.1.2...HEAD
[0.1.2]: https://github.com/Overmock/Overmock/compare/v0.1.1...v0.1.2
[0.1.1]: https://github.com/Overmock/Overmock/compare/v0.1.0...v0.1.1
[0.1.0]: https://github.com/Overmock/Overmock/releases/tag/v0.1.0
