using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Xunit;

namespace ServiceModel.Configuration.ConfigurationManager.Tests
{
    public class BindingTests : ServiceModelTestBase
    {
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
    }
}
