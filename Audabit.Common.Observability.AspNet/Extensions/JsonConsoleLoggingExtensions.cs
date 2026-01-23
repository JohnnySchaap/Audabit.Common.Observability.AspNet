using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Audabit.Common.Observability.AspNet.Extensions;

/// <summary>
/// Provides extension methods for configuring JSON console logging.
/// </summary>
public static class JsonConsoleLoggingExtensions
{
    /// <summary>
    /// Adds JSON console logging with scopes and timestamp formatting.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is null.</exception>
    /// <remarks>
    /// <para>
    /// This method adds a JSON console logging provider with the following settings:
    /// <list type="bullet">
    /// <item><description>Adds JSON console formatter (does not clear existing providers)</description></item>
    /// <item><description>Enables scopes for correlation tracking</description></item>
    /// <item><description>Sets timestamp format to "yyyy-MM-dd HH:mm:ss "</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// This method is additive and preserves any existing logging providers.
    /// To replace all providers with JSON console only, use <see cref="UseJsonConsoleLogging"/> instead.
    /// </para>
    /// </remarks>
    /// <example>
    /// Add JSON console logging alongside existing providers:
    /// <code>
    /// builder.Services
    ///     .AddObservability("MyService")
    ///     .AddJsonConsoleLogging();
    /// </code>
    /// </example>
    public static IServiceCollection AddJsonConsoleLogging(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddLogging(builder => builder.AddJsonConsole(options =>
            {
                options.IncludeScopes = true;
                options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
            }));

        return services;
    }

    /// <summary>
    /// Replaces all existing logging providers with JSON console logging.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is null.</exception>
    /// <remarks>
    /// <para>
    /// This method clears all existing logging providers and configures JSON console logging with the following settings:
    /// <list type="bullet">
    /// <item><description>Clears all existing providers</description></item>
    /// <item><description>Adds JSON console formatter</description></item>
    /// <item><description>Enables scopes for correlation tracking</description></item>
    /// <item><description>Sets timestamp format to "yyyy-MM-dd HH:mm:ss "</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Use this method when you want JSON console logging as the only provider.
    /// To add JSON console logging alongside existing providers, use <see cref="AddJsonConsoleLogging"/> instead.
    /// </para>
    /// </remarks>
    /// <example>
    /// Replace all logging with JSON console only:
    /// <code>
    /// builder.Services
    ///     .AddObservability("MyService")
    ///     .UseJsonConsoleLogging();
    /// </code>
    /// </example>
    public static IServiceCollection UseJsonConsoleLogging(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddLogging(builder => builder.ClearProviders());

        return services.AddJsonConsoleLogging();
    }
}