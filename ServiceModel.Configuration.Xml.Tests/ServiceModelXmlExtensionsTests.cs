using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.IO;
using Xunit;

namespace ServiceModel.Configuration.Xml.Tests
{
    public class ServiceModelXmlExtensionsTests
    {
        [Fact]
        public void SingleService()
        {
            var xml = @"
<configuration>
    <system.serviceModel>
        <services>
            <service name=""service1"">
                <endpoint
                    address=""http://service1""
                    contract=""IService"" />
            </service>
        </services>
    </system.serviceModel>
</configuration>";

            using (var fs = TemporaryFileStream.Create(xml))
            {
                void Configure(ServiceModelBuilder builder)
                {
                    var mapper = Substitute.For<ITypeMapper>();

                    mapper.GetType("IService").Returns(typeof(IService));

                    builder.Services.AddSingleton<ITypeMapper>(mapper);
                    builder.AddXmlConfiguration(fs.Name);
                }

                using (var provider = CreateProvider(Configure))
                {
                    var factoryProvider = provider.GetRequiredService<IChannelFactoryProvider>();
                    var factory = factoryProvider.CreateChannelFactory<IService>("service1");

                    Assert.Equal("http://service1/", factory.Endpoint.Address.ToString());
                }
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
