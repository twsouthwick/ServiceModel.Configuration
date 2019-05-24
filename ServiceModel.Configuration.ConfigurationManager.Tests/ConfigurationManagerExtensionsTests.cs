using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
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

        [InlineData("basicHttpBinding", typeof(BasicHttpBinding))]
        [InlineData("basicHttpsBinding", typeof(BasicHttpsBinding))]
        [Theory]
        public void SimpleBasicHttpBinding(string bindingName, Type bindingType)
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
                    contract=""{contract}""
                    binding=""{bindingName}"" />
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
                    Assert.Equal(bindingType, factory.Endpoint.Binding.GetType());
                }
            }
        }

        [Fact]
        public void CustomBinding()
        {
            var name = Create<string>();
            var address = Create<Uri>().ToString();
            var contract = Create<string>();
            var xml = $@"
<configuration>
    <system.serviceModel>
        <bindings>
            <customBinding>
                <binding name=""customName"">
                    <binaryMessageEncoding compressionFormat=""GZip"" />
                    <sslStreamSecurity requireClientCertificate=""true"" />
                </binding>
            </customBinding>
        </bindings>
        <services>
            <service name=""service1"">
                <endpoint address=""{address}""
                    binding=""customBinding"" bindingConfiguration=""customName""
                    contract=""{contract}"">
                </endpoint>
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
                    var binding = Assert.IsType<CustomBinding>(factory.Endpoint.Binding);
                }
            }
        }

#if FALSE
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

        private class TemporaryFileStream : FileStream
        {
            private TemporaryFileStream(string path)
                : base(path, FileMode.Open, FileAccess.Read)
            {
            }

            public static FileStream Create(string xml)
            {
                var path = Path.GetTempFileName();

                File.WriteAllText(path, xml);

                return new TemporaryFileStream(path);
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (disposing)
                {
                    File.Delete(Name);
                }
            }
        }
    }
}
