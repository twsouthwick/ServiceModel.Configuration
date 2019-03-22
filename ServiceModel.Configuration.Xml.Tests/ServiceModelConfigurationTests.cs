using ServiceModel.Configuration.Xml;
using System.IO;
using System.Xml;
using Xunit;

namespace ServiceModel.Configuration.Tests
{
    public class ServiceModelConfigurationTests : ServiceModelTestBase
    {
        [Fact]
        public void EmptyModel()
        {
            using (var reader = CreateReader("<root/>"))
            {
                var config = ServiceModelConfiguration.Parse(reader);

                Assert.Empty(config.Bindings);
                Assert.Empty(config.ContractBehaviors);
                Assert.Empty(config.EndpointBehaviors);
                Assert.Empty(config.OperationBehaviors);
                Assert.Empty(config.Services);
            }
        }

        [Fact]
        public void SingleService()
        {
            var xml = @"
<configuration>
    <system.serviceModel>
        <services>
            <service name=""Service1"">
                <endpoint
                    address=""http://service1""
                    contract=""MyContract"" />
            </service>
        </services>
    </system.serviceModel>
</configuration>";

            using (var reader = CreateReader(xml))
            {
                var config = ServiceModelConfiguration.Parse(reader);

                Assert.Empty(config.Bindings);
                Assert.Empty(config.ContractBehaviors);
                Assert.Empty(config.EndpointBehaviors);
                Assert.Empty(config.OperationBehaviors);

                var service = Assert.Single(config.Services);

                Assert.Equal("Service1", service.Name);
                var endpoint = Assert.Single(service.Endpoints);

                Assert.Equal("http://service1", endpoint.Address);
                Assert.Equal("MyContract", endpoint.Contract);
                Assert.Empty(endpoint.Binding);
                Assert.Empty(endpoint.BehaviorConfiguration);
            }
        }

        private static XmlReader CreateReader(string input) => XmlReader.Create(new StringReader(input));
    }
}
