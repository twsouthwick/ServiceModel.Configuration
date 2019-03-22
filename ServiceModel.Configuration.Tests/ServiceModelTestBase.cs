using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ServiceModel;

namespace ServiceModel.Configuration
{
    public class ServiceModelTestBase
    {
        private readonly Fixture _fixture;

        protected ServiceModelTestBase()
        {
            _fixture = new Fixture();

            _fixture.Register<string>(() => $"string_{Guid.NewGuid()}");
            _fixture.Register<EndpointAddress>(() => new EndpointAddress(_fixture.Create<Uri>().ToString()));
            _fixture.Register<Uri>(() => new Uri($"http://{Guid.NewGuid()}"));
        }

        protected T Create<T>() => _fixture.Create<T>();

        protected interface IService
        {
        }

        protected ServiceProvider CreateProvider(Action<ServiceModelBuilder> configure)
        {
            var services = new ServiceCollection();

            configure(services.AddServiceModelClient());

            return services.BuildServiceProvider();
        }
    }
}
