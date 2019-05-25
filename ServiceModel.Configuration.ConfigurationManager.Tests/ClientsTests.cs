using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
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

#if DESKTOP
        [Fact]
        public void Example()
        {
            var name = Create<string>();
            var address = Create<Uri>().ToString();
            var contract = Create<string>();
            var xml = $@"
<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration>
  <system.serviceModel>
    <bindings>
      <customBinding>
        <binding name=""CompressedTcpBinding_BeanTraderService"">
          <binaryMessageEncoding compressionFormat=""GZip"" />
          <!-- Was going to use TransportWithMessageCredential security, but
               that is not yet supported on .NET Core. https://github.com/dotnet/wcf/issues/8 -->
          <sslStreamSecurity requireClientCertificate=""true"" />
          <tcpTransport />
        </binding>
      </customBinding>
      <!--
      <netTcpBinding>
        <binding name=""NetTcpBinding_BeanTraderService"">
        <security mode=""Transport"">
          <transport clientCredentialType=""Certificate"" />
        </security>
        </binding>
      </netTcpBinding>
        -->
      </bindings>
    <client>
      <endpoint address=""{address}""
          binding=""customBinding"" bindingConfiguration=""CompressedTcpBinding_BeanTraderService""
          contract=""BeanTraderService"" name=""CompressedTcpBinding_BeanTraderService"">
        <identity>
          <dns value=""BeanTrader"" />
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
                    var factory = factoryProvider.CreateChannelFactory<IService>(name);

                    Assert.Equal(address, factory.Endpoint.Address.ToString());
                    Assert.Equal(bindingType, factory.Endpoint.Binding.GetType());
                }
            }
        }
#endif
    }
}
