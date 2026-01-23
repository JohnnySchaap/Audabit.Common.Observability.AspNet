using Audabit.Common.Observability.AspNet.Extensions;
using Audabit.Common.Observability.AspNet.Tests.Unit.TestHelpers;
using Audabit.Common.Observability.Emitters;
using Microsoft.Extensions.DependencyInjection;

namespace Audabit.Common.Observability.AspNet.Tests.Unit.Extensions;

public class ObservabilityExtensionsTests
{
    public class ObservabilityExtensionsTestsBase
    {
        protected readonly Fixture _fixture;
        protected readonly ServiceCollection _services;

        public ObservabilityExtensionsTestsBase()
        {
            _fixture = FixtureFactory.Create();
            _services = new ServiceCollection();
            _services.AddLogging();
        }
    }

    public class AddObservability : ObservabilityExtensionsTestsBase
    {
        [Fact]
        public void GivenValidServices_ShouldRegisterEmitterAsSingleton()
        {
            // Act
            var result = _services.AddObservability(_fixture.Create<string>());

            // Assert
            result.ShouldBe(_services);
            var serviceProvider = _services.BuildServiceProvider();
            var emitter1 = serviceProvider.GetService<IEmitter<ObservabilityExtensionsTests>>();
            var emitter2 = serviceProvider.GetService<IEmitter<ObservabilityExtensionsTests>>();

            emitter1.ShouldNotBeNull();
            emitter1.ShouldBeSameAs(emitter2);
        }
    }
}