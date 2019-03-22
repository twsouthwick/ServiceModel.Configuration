using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using Xunit;

namespace ServiceModel.Configuration.Tests
{
    public class ServiceModelBuilderTests : ServiceModelTestBase
    {
        [Fact]
        public void NoEndpointRegistered()
        {
            using (var provider = CreateProvider(wcf => { }))
            {
                var factoryProvider = provider.GetRequiredService<IChannelFactoryProvider>();

                Assert.Throws<ArgumentNullException>(() => factoryProvider.CreateChannelFactory<IService>(null));
                Assert.Throws<ServiceConfigurationException>(() => factoryProvider.CreateChannelFactory<IService>(string.Empty));
                Assert.Throws<ServiceConfigurationException>(() => factoryProvider.CreateChannelFactory<IService>(Create<string>()));
            }
        }

        [Fact]
        public void RegisteredEndpoint()
        {
            var endpoint = Create<EndpointAddress>();
            var name = Create<string>();

            void Configure(ServiceModelBuilder wcf)
            {
                wcf.AddServiceEndpoint(name, e =>
                {
                    e.Services.Add<IService>(s =>
                    {
                        s.Endpoint = endpoint;
                    });
                });
            }

            using (var provider = CreateProvider(Configure))
            {
                var factoryProvider = provider.GetRequiredService<IChannelFactoryProvider>();
                var factory = factoryProvider.CreateChannelFactory<IService>(name);

                Assert.Equal(endpoint, factory.Endpoint.Address);
            }
        }

        [Fact]
        public void RegisteredEndpointTwice()
        {
            var endpoint = Create<EndpointAddress>();
            var name = Create<string>();

            void Configure(ServiceModelBuilder wcf)
            {
                wcf.AddServiceEndpoint(name, e =>
                {
                    e.Services.Add<IService>(s =>
                    {
                        s.Endpoint = endpoint;
                    });
                });

                wcf.AddServiceEndpoint(name, e =>
                {
                    e.Services.Add<IService>(s =>
                    {
                    });
                });
            }

            using (var provider = CreateProvider(Configure))
            {
                var factoryProvider = provider.GetRequiredService<IChannelFactoryProvider>();
                var factory = factoryProvider.CreateChannelFactory<IService>(name);

                Assert.Equal(endpoint, factory.Endpoint.Address);
            }
        }

        [Fact]
        public void CustomBehavior()
        {
            var name = Create<string>();
            var behavior = Substitute.For<IEndpointBehavior>();

            void Configure(ServiceModelBuilder wcf)
            {
                wcf.AddServiceEndpoint(name, e =>
                {
                    e.Services.Add<IService>(s =>
                    {
                        s.Endpoint = Create<EndpointAddress>();

                        s.Behaviors.Add(behavior);
                    });
                });
            }

            using (var provider = CreateProvider(Configure))
            {
                var factoryProvider = provider.GetRequiredService<IChannelFactoryProvider>();
                var factory = factoryProvider.CreateChannelFactory<IService>(name);

                Assert.Equal(2, factory.Endpoint.EndpointBehaviors.Count);
                Assert.Single(factory.Endpoint.EndpointBehaviors.Where(b => b == behavior));
            }
        }

        [Fact]
        public void GlobalCustomBehavior()
        {
            var name = Create<string>();
            var behavior = Substitute.For<IEndpointBehavior>();

            void Configure(ServiceModelBuilder wcf)
            {
                wcf.AddServiceEndpoint(name, e =>
                {
                    e.Services.Add<IService>(s =>
                    {
                        s.Endpoint = Create<EndpointAddress>();
                    });
                });

                wcf.AddGlobalBehavior(behavior);
            }

            using (var provider = CreateProvider(Configure))
            {
                var factoryProvider = provider.GetRequiredService<IChannelFactoryProvider>();
                var factory = factoryProvider.CreateChannelFactory<IService>(name);

                Assert.Equal(2, factory.Endpoint.EndpointBehaviors.Count);
                Assert.Single(factory.Endpoint.EndpointBehaviors.Where(b => b == behavior));
            }
        }
    }
}
