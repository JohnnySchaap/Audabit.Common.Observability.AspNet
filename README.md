# Audabit.Common.Observability.AspNet

ASP.NET Core integration for Audabit.Common.Observability providing automatic emitter registration and JSON console logging.

## Why You Should Use Structured Observability

Logging to the console with `Console.WriteLine` or unstructured log messages makes it nearly impossible to query and analyze logs in production. Modern cloud environments need structured, searchable logging from day one.

### Searchable, Queryable Logs

JSON-formatted logs can be searched and filtered by any field in logging aggregation tools like Application Insights, CloudWatch, or ELK. Unstructured text logs require complex regex patterns that often miss edge cases. Structured logging lets you query by service name, correlation ID, log level, or custom properties instantly.

### Consistent Logging Pattern

The `IEmitter<T>` pattern provides a consistent way to emit structured log entries across your entire application. Instead of remembering logger syntax and structured logging APIs, you define your log types and emit them. The library handles formatting, enrichment, and output configuration.

### Cloud-Native by Default

Cloud platforms and container orchestration expect JSON logs on stdout. This library configures your application for cloud deployment automatically—no need to set up different logging for local development versus production.

## Dependencies

This library depends on the following Audabit packages:
- **Audabit.Common.Observability** - Core observability library providing the `IEmitter<T>` pattern

## Library Design Principles

> **ConfigureAwait Best Practices**: This library follows Microsoft's recommended async/await best practices by using `ConfigureAwait(false)` on all await statements. This eliminates unnecessary context switches and improves performance by allowing continuations to run on any thread pool thread rather than marshaling back to the original synchronization context.

## Features

- **Automatic Emitter Registration**: Registers `IEmitter<T>` as singleton for dependency injection
- **JSON Console Logging**: Structured JSON logging for cloud-native applications
- **Service Name Configuration**: Configure service name from settings or directly
- **Logging Customization**: Customize JSON console logging options
- **.NET 10.0 Support**: Built for the latest .NET framework

## Installation

### Via .NET CLI

```bash
dotnet add package Audabit.Common.Observability.AspNet
```

### Via Package Manager Console

```powershell
Install-Package Audabit.Common.Observability.AspNet
```

## Getting Started

### Basic Usage

Register observability in `Program.cs`:

```csharp
using Audabit.Common.Observability.AspNet.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register observability with service name
builder.Services.AddObservability("MyService.WebApi");

var app = builder.Build();
app.Run();
```

### Configuration-Based Registration

```csharp
using Audabit.Common.Observability.AspNet.Extensions;

var serviceSettingsSection = builder.Configuration.GetSection(nameof(ServiceSettings));
builder.Services.AddObservability<ServiceSettings>(
    serviceSettingsSection,
    settings => settings?.ServiceName);
```

### JSON Console Logging

```csharp
// Use JSON console logging (replaces default console logging)
builder.Services.UseJsonConsoleLogging();

// Or customize logging configuration
builder.Services.AddJsonConsoleLogging(logging =>
{
    logging.SetMinimumLevel(LogLevel.Debug);
});
```

## How It Works

The `AddObservability` extension method:

1. Registers `IEmitter<T>` as a singleton in the dependency injection container
2. Sets the service name for all logging events
3. Enables structured logging with event properties
4. Integrates with ASP.NET Core's logging infrastructure

## Related Packages

This library works seamlessly with other Audabit packages:

- **[Audabit.Common.Observability](https://dev.azure.com/johnnyschaap/Audabit/_artifacts/feed/Audabit/NuGet/Audabit.Common.Observability)**: Core observability library providing the `IEmitter<T>` pattern (required dependency)
- **[Audabit.Common.CorrelationId.AspNet](https://dev.azure.com/johnnyschaap/Audabit/_artifacts/feed/Audabit/NuGet/Audabit.Common.CorrelationId.AspNet)**: Adds correlation ID tracking to structured logs for distributed tracing
- **[Audabit.Common.ExceptionHandling.AspNet](https://dev.azure.com/johnnyschaap/Audabit/_artifacts/feed/Audabit/NuGet/Audabit.Common.ExceptionHandling.AspNet)**: Global exception handling with structured error logging

This package provides ASP.NET Core integration for the core Audabit.Common.Observability library, enabling structured logging and telemetry in web applications.

## Build and Test

### Prerequisites

- .NET 10.0 SDK or later
- Visual Studio 2022 / VS Code / Rider (optional)

### Building

```bash
dotnet restore
dotnet build
```

### Running Tests

```bash
dotnet test
```

### Creating NuGet Package

```bash
dotnet pack --configuration Release
```

## CI/CD Pipeline

This project uses Azure DevOps pipelines with the following features:

- **Automatic Versioning**: Major and minor versions from csproj, patch version from build number
- **Prerelease Builds**: Non-main branches create prerelease packages (e.g., `9.0.123-feature-auth`)
- **Code Formatting**: Enforces `dotnet format` standards
- **Code Coverage**: Generates and publishes code coverage reports
- **Automated Publishing**: Pushes packages to Azure Artifacts feed

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

### Development Guidelines

1. Follow existing code style and conventions
2. Ensure all tests pass before submitting PR
3. Add tests for new features
4. Update documentation as needed
5. Run `dotnet format` before committing

## License

Copyright © Audabit Software Solutions B.V. 2026

Licensed under the Apache License, Version 2.0. See [LICENSE](LICENSE) file for details.

## Authors

- [John Schaap](https://github.com/JohnnySchaap) - [Audabit Software Solutions B.V.](https://audabit.nl)

## Acknowledgments

- Built on top of Audabit.Common.Observability
- Designed for [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet)