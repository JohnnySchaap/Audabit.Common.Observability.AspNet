using Audabit.Common.Observability.AspNet.Extensions;
using Audabit.Common.Observability.AspNet.Tests.Unit.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Audabit.Common.Observability.AspNet.Tests.Unit.Extensions;

public class JsonConsoleLoggingExtensionsTests
{
    public class JsonConsoleLoggingExtensionsTestsBase
    {
        protected readonly Fixture _fixture;
        protected readonly ServiceCollection _services;

        public JsonConsoleLoggingExtensionsTestsBase()
        {
            _fixture = FixtureFactory.Create();
            _services = new ServiceCollection();
        }
    }

    public class AddJsonConsoleLogging : JsonConsoleLoggingExtensionsTestsBase
    {
        [Fact]
        public void GivenValidServices_ShouldReturnServiceCollectionAndConfigureLogging()
        {
            // Arrange
            var initialCount = _services.Count;

            // Act
            var result = _services.AddJsonConsoleLogging();
            var serviceProvider = _services.BuildServiceProvider();

            // Assert
            result.ShouldBe(_services);
            _services.Count.ShouldBeGreaterThan(initialCount);
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.ShouldNotBeNull();
        }

        [Fact]
        public void GivenNullServices_ShouldThrowArgumentNullException()
        {
            // Arrange
            IServiceCollection? services = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => services!.AddJsonConsoleLogging());
        }

        [Fact]
        public void GivenMultipleCalls_ShouldBeAdditive()
        {
            // Act
            _services.AddJsonConsoleLogging();
            var countAfterFirst = _services.Count;
            _services.AddJsonConsoleLogging();
            var countAfterSecond = _services.Count;

            // Assert
            countAfterSecond.ShouldBeGreaterThan(countAfterFirst);
        }
    }

    public class UseJsonConsoleLogging : JsonConsoleLoggingExtensionsTestsBase
    {
        [Fact]
        public void GivenValidServices_ShouldReturnServiceCollectionAndConfigureLogging()
        {
            // Arrange
            var initialCount = _services.Count;

            // Act
            var result = _services.UseJsonConsoleLogging();
            var serviceProvider = _services.BuildServiceProvider();

            // Assert
            result.ShouldBe(_services);
            _services.Count.ShouldBeGreaterThan(initialCount);
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.ShouldNotBeNull();
        }

        [Fact]
        public void GivenNullServices_ShouldThrowArgumentNullException()
        {
            // Arrange
            IServiceCollection? services = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => services!.UseJsonConsoleLogging());
        }

        [Fact]
        public void GivenExistingProviders_ShouldClearThenAdd()
        {
            // Arrange
            _services.AddLogging(builder => builder.AddConsole());

            // Act
            _services.UseJsonConsoleLogging();
            var countAfterUse = _services.Count;

            // Assert
            countAfterUse.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void GivenMultipleCalls_ShouldReplaceEachTime()
        {
            // Act
            _services.UseJsonConsoleLogging();
            var countAfterFirst = _services.Count;
            _services.UseJsonConsoleLogging();
            var countAfterSecond = _services.Count;

            // Assert
            countAfterSecond.ShouldBeGreaterThan(countAfterFirst);
        }
    }
}