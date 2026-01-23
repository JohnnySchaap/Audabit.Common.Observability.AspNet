using Audabit.Common.Observability.Emitters;
using Audabit.Common.Observability.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Audabit.Common.Observability.AspNet.Extensions;

/// <summary>
/// Provides extension methods for registering observability services in ASP.NET Core applications.
/// </summary>
public static class ObservabilityServiceExtensions
{
    /// <summary>
    /// Registers observability services with an explicit service name.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="serviceName">The name of the service for logging categorization.</param>
    /// <returns>The <see cref="IServiceCollection"/> for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="serviceName"/> is null or whitespace.</exception>
    /// <remarks>
    /// <para>
    /// This method performs the following registrations:
    /// <list type="bullet">
    /// <item><description>Registers <see cref="IEmitter{T}"/> as a singleton using open generic pattern</description></item>
    /// <item><description>Sets the global service name for all <see cref="LoggingEvent"/> instances</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// This method does NOT configure logging providers. Use <see cref="JsonConsoleLoggingExtensions.AddJsonConsoleLogging"/> 
    /// or configure logging separately based on your requirements (e.g., Application Insights, Serilog).
    /// </para>
    /// </remarks>
    /// <example>
    /// Register observability services in Program.cs:
    /// <code>
    /// builder.Services
    ///     .AddObservability("Audabit.Service.DesignPatterns.WebApi")
    ///     .AddJsonConsoleLogging();
    /// </code>
    /// 
    /// Or configure your own logging:
    /// <code>
    /// builder.Services
    ///     .AddObservability("MyService");
    /// 
    /// builder.Logging.AddApplicationInsights();
    /// </code>
    /// </example>
    public static IServiceCollection AddObservability(this IServiceCollection services, string? serviceName)
    {
        ArgumentNullException.ThrowIfNull(services);
        serviceName = string.IsNullOrWhiteSpace(serviceName) ? "UnknownService" : serviceName;

        services.AddSingleton(typeof(IEmitter<>), typeof(Emitter<>));

        LoggingEvent.SetServiceName(serviceName);

        return services;
    }

    /// <summary>
    /// Registers observability services with service name extracted from configuration.
    /// </summary>
    /// <typeparam name="TSettings">The settings type containing service name property.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configurationSection">The configuration section containing settings.</param>
    /// <param name="serviceNameSelector">Function to extract service name from settings.</param>
    /// <returns>The <see cref="IServiceCollection"/> for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/>, <paramref name="configurationSection"/>, or <paramref name="serviceNameSelector"/> is null.</exception>
    /// <remarks>
    /// <para>
    /// This overload reads the service name from configuration instead of requiring it explicitly.
    /// If the service name cannot be extracted or is null, "UnknownService" is used as a fallback.
    /// </para>
    /// <para>
    /// This method does NOT configure logging providers. Use <see cref="JsonConsoleLoggingExtensions.AddJsonConsoleLogging"/> 
    /// or configure logging separately based on your requirements.
    /// </para>
    /// </remarks>
    /// <example>
    /// Register in Program.cs with service name from configuration:
    /// <code>
    /// var serviceSettingsSection = builder.Configuration.GetSection(nameof(ServiceSettings));
    /// builder.Services
    ///     .AddObservability&lt;ServiceSettings&gt;(
    ///         serviceSettingsSection,
    ///         settings => settings?.ServiceName)
    ///     .AddJsonConsoleLogging();
    /// </code>
    /// </example>
    public static IServiceCollection AddObservability<TSettings>(
        this IServiceCollection services,
        IConfigurationSection configurationSection,
        Func<TSettings?, string?> serviceNameSelector)
        where TSettings : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configurationSection);
        ArgumentNullException.ThrowIfNull(serviceNameSelector);

        var settings = configurationSection.Get<TSettings>();
        var serviceName = serviceNameSelector(settings);

        return services.AddObservability(serviceName);
    }
}