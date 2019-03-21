using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace ServiceModel.Configuration.Xml.Tests
{
    public class ServiceModelXmlExtensionsTests
    {
        [Fact(Skip = "Not implemented yet")]
        public void XmlConfiguration()
        {
            void Configure(ServiceModelBuilder builder)
            {
                builder.AddXmlConfiguration("config1.xml");
            }

            using (var provider = CreateProvider(Configure))
            {
                var factoryProvider = provider.GetRequiredService<IChannelFactoryProvider>();
                var factory = factoryProvider.CreateChannelFactory<IService>("service1");

                Assert.Single(factory.Endpoint.EndpointBehaviors);
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
