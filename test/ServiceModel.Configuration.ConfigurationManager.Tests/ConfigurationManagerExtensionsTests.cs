using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.ServiceModel.Description;
using Xunit;

namespace ServiceModel.Configuration.ConfigurationManager.Tests
{
    public class ConfigurationManagerExtensionsTests : ServiceModelTestBase
    {
        [Fact]
        public void SingleServiceDefaultResolver()
        {
            var name = Create<string>();
            var address = Create<Uri>().ToString();
            var xml = $@"
<configuration>
    <system.serviceModel>
          <services>
              <service name=""{name}"">
                  <endpoint
                      address=""{address}""
                      contract=""{typeof(IService).FullName}"" />
              </service>
          </services>
      </system.serviceModel>
</configuration>";

            using (var fs = TemporaryFileStream.Create(xml))
            {
                void Configure(ServiceModelBuilder builder)
                {
                    builder.AddConfigurationManagerFile(fs.Name);
                }

                using (var provider = CreateProvider(Configure))
                {
                    var factoryProvider = provider.GetRequiredService<IChannelFactoryProvider>();
                    var factory = factoryProvider.CreateChannelFactory<IService>(name);

                    Assert.Equal(address, factory.Endpoint.Address.ToString());
                }
            }
        }

        [Fact]
        public void SingleServiceDefaultResolverDifferentName()
        {
            var name1 = Create<string>();
            var name2 = Create<string>();
            var address1 = Create<Uri>().ToString();
            var address2 = Create<Uri>().ToString();
            var xml = $@"
<configuration>
    <system.serviceModel>
          <services>
              <service name=""{name1}"">
                  <endpoint
                      address=""{address1}""
                      contract=""{typeof(IService).FullName}"" />
              </service>
              <service name=""{name2}"">
                  <endpoint
                      address=""{address2}""
                      contract=""{typeof(IService).FullName}"" />
              </service>
          </services>
      </system.serviceModel>
</configuration>";

            using (var fs = TemporaryFileStream.Create(xml))
            {
                void Configure(ServiceModelBuilder builder)
                {
                    builder.AddConfigurationManagerFile(fs.Name);
                }

                using (var provider = CreateProvider(Configure))
                {
                    var factoryProvider = provider.GetRequiredService<IChannelFactoryProvider>();

                    var factory1 = factoryProvider.CreateChannelFactory<IService>(name1);
                    Assert.Equal(address1, factory1.Endpoint.Address.ToString());

                    var factory2 = factoryProvider.CreateChannelFactory<IService>(name2);
                    Assert.Equal(address2, factory2.Endpoint.Address.ToString());
                }
            }
        }

        [Fact]
        public void SingleServiceCustomResolver()
        {
            var name = Create<string>();
            var address = Create<Uri>().ToString();
            var contract = Create<string>();
            var xml = $@"
<configuration>
    <system.serviceModel>
        <services>
            <service name=""{name}"">
                <endpoint
                    address=""{address}""
                    contract=""{contract}"" />
            </service>
        </services>
    </system.serviceModel>
</configuration>";

            using (var fs = TemporaryFileStream.Create(xml))
            {
                void Configure(ServiceModelBuilder builder)
                {
                    var mapper = Substitute.For<IContractResolver>();

                    mapper.ResolveContract(contract).Returns(typeof(IService));
                    mapper.ResolveDescription(typeof(IService)).Returns(ContractDescription.GetContract(typeof(IService)));

                    builder.Services.AddSingleton<IContractResolver>(mapper);
                    builder.AddConfigurationManagerFile(fs.Name);
                }

                using (var provider = CreateProvider(Configure))
                {
                    var factoryProvider = provider.GetRequiredService<IChannelFactoryProvider>();
                    var factory = factoryProvider.CreateChannelFactory<IService>(name);

                    Assert.Equal(address, factory.Endpoint.Address.ToString());
                }
            }
        }
    }
}