using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ServiceModel.Configuration.Xml;

namespace ServiceModel.Configuration
{
    public static class ServiceModelXmlExtensions
    {
        public static ServiceModelBuilder AddXmlConfiguration(this ServiceModelBuilder builder, string path)
        {
            builder.Services.AddSingleton<IConfigureNamedOptions<ServiceModelOptions>>(new XmlServiceModelOptions(path));

            return builder;
        }
    }
}
