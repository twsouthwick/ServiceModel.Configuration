using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using Xunit;

namespace ServiceModel.Configuration.Tests
{
    public class ServiceModelBuilderTests
    {
        private readonly Fixture _fixture;

        public ServiceModelBuilderTests()
        {
            _fixture = new Fixture();

            _fixture.Register<string>(() => $"string_{Guid.NewGuid()}");
            _fixture.Register<EndpointAddress>(() => new EndpointAddress(_fixture.Create<Uri>().ToString()));
            _fixture.Register<Uri>(() => new Uri($"http://{Guid.NewGuid()}"));
        }

        [Fact]
        public void NoEndpointRegistered()
        {
            using (var provider = CreateProvider(wcf => { }))
            {
                var factoryProvider = provider.GetRequiredService<IChannelFactoryProvider<IService>>();

                Assert.Throws<ArgumentNullException>(() => factoryProvider.CreateChannelFactory(null));
                Assert.Throws<UnknownEndpointException>(() => factoryProvider.CreateChannelFactory(string.Empty));
                Assert.Throws<UnknownEndpointException>(() => factoryProvider.CreateChannelFactory(_fixture.Create<string>()));
            }
        }

        [Fact]
        public void RegisteredEndpoint()
        {
            var endpoint = _fixture.Create<EndpointAddress>();
            var name = _fixture.Create<string>();

            void Configure(ServiceModelBuilder wcf)
            {
                wcf.AddServiceEndpoint(name, e =>
                {
                    e.Endpoint = endpoint;
                });
            }

            using (var provider = CreateProvider(Configure))
            {
                var factoryProvider = provider.GetRequiredService<IChannelFactoryProvider<IService>>();
                var factory = factoryProvider.CreateChannelFactory(name);

                Assert.Equal(endpoint, factory.Endpoint.Address);
            }
        }

        [Fact]
        public void CustomBehavior()
        {
            var name = _fixture.Create<string>();
            var behavior = Substitute.For<IEndpointBehavior>();

            void Configure(ServiceModelBuilder wcf)
            {
                wcf.AddServiceEndpoint(name, e =>
                {
                    e.Endpoint = _fixture.Create<EndpointAddress>();
                    e.Behaviors.Add(behavior);
                });
            }

            using (var provider = CreateProvider(Configure))
            {
                var factoryProvider = provider.GetRequiredService<IChannelFactoryProvider<IService>>();
                var factory = factoryProvider.CreateChannelFactory(name);

                Assert.Equal(2, factory.Endpoint.EndpointBehaviors.Count);
                Assert.Single(factory.Endpoint.EndpointBehaviors.Where(b => b == behavior));
            }
        }

        [Fact]
        public void GlobalCustomBehavior()
        {
            var name = _fixture.Create<string>();
            var behavior = Substitute.For<IEndpointBehavior>();

            void Configure(ServiceModelBuilder wcf)
            {
                wcf.AddServiceEndpoint(name, e =>
                {
                    e.Endpoint = _fixture.Create<EndpointAddress>();
                });

                wcf.AddGlobalBehavior(behavior);
            }

            using (var provider = CreateProvider(Configure))
            {
                var factoryProvider = provider.GetRequiredService<IChannelFactoryProvider<IService>>();
                var factory = factoryProvider.CreateChannelFactory(name);

                Assert.Equal(2, factory.Endpoint.EndpointBehaviors.Count);
                Assert.Single(factory.Endpoint.EndpointBehaviors.Where(b => b == behavior));
            }
        }

        private interface IService
        {
        }

        public ServiceProvider CreateProvider(Action<ServiceModelBuilder> configure)
        {
            var services = new ServiceCollection();

            configure(services.AddServiceModelClient());

            return services.BuildServiceProvider();
        }
    }
}
