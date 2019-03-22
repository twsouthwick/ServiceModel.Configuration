using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ServiceModel.Configuration.Xml;
using System.Xml;

namespace ServiceModel.Configuration
{
    public static class ServiceModelXmlExtensions
    {
        public static ServiceModelBuilder AddXmlConfiguration(this ServiceModelBuilder builder, string path)
        {
            using (var reader = XmlReader.Create(path))
            {
                var configuration = ServiceModelConfiguration.Parse(reader);

                builder.Services.AddSingleton<IConfigureOptions<ServiceModelOptions>>(ctx => new XmlServiceModelOptions(ctx.GetRequiredService<ITypeMapper>(), configuration));

                return builder;
            }
        }
    }
}
