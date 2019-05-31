using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Xunit;

namespace ServiceModel.Configuration.ConfigurationManager.Tests
{
    public class ClientsTests : ServiceModelTestBase
    {
        [Fact]
        public void ClientBlock()
        {
            var address = Create<Uri>().ToString();
            var contract = Create<string>();
            var xml = $@"
<configuration>
  <system.serviceModel>
    <client>
      <endpoint address=""{address}""
          binding=""basicHttpBinding""
          contract=""{contract}"">
      </endpoint>
    </client>
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
                    var factory = factoryProvider.CreateChannelFactory<IService>();

                    Assert.Equal(address, factory.Endpoint.Address.ToString());
                }
            }
        }

        [Fact]
        public void Example()
        {
            var dns = Create<string>();
            var address = Create<Uri>().ToString();
            var contract = Create<string>();
            var xml = $@"
<configuration>
  <system.serviceModel>
    <client>
      <endpoint address=""{address}""
          binding=""basicHttpBinding""
          contract=""{contract}"">
        <identity>
          <dns value=""{dns}"" />
        </identity>
      </endpoint>
    </client>
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
                    var factory = factoryProvider.CreateChannelFactory<IService>();

                    Assert.Equal(address, factory.Endpoint.Address.ToString());

                    var identity = Assert.IsAssignableFrom<EndpointIdentity>(Assert.Single(factory.Endpoint.EndpointBehaviors));
                }
            }
        }
    }
}
