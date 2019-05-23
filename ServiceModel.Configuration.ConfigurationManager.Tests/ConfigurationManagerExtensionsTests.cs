using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.IO;
using System.ServiceModel;
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

        [Fact]
        public void SimpleBasicHttpBinding()
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
                    binding=""basicHttpBinding"" />
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
                    Assert.IsType<BasicHttpBinding>(factory.Endpoint.Binding);
                }
            }
        }

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
