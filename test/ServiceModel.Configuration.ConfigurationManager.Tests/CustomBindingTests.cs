using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Security.Authentication;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using Xunit;

namespace ServiceModel.Configuration.ConfigurationManager.Tests
{
    public class CustomBindingTests : ServiceModelTestBase
    {
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
            <service name=""{name}"">
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

                    Assert.Collection(binding.Elements,
                        e =>
                        {
                            var encoding = Assert.IsType<BinaryMessageEncodingBindingElement>(e);

                            Assert.Equal(CompressionFormat.GZip, encoding.CompressionFormat);
                        },
                        e =>
                        {
                            var ssl = Assert.IsType<SslStreamSecurityBindingElement>(e);

                            Assert.Equal(SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12, ssl.SslProtocols);
                        });
                }
            }
        }

        [Fact]
        public void CustomBindingWithTcpTransport()
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
                    <tcpTransport />
                </binding>
            </customBinding>
        </bindings>
        <services>
            <service name=""{name}"">
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

                    Assert.Collection(binding.Elements, e => Assert.IsType<TcpTransportBindingElement>(e));
                }
            }
        }
    }
}
