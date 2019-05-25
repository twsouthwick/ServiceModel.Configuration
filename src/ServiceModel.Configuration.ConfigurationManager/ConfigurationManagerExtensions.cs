using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ServiceModel.Configuration
{
    public static class ConfigurationManagerExtensions
    {
        public static ServiceModelBuilder AddConfigurationManagerFile(this ServiceModelBuilder builder, string path, bool isOptional = false)
        {
            builder.Services.AddSingleton<IConfigureOptions<ServiceModelOptions>>(ctx => new ConfigurationManagerServiceModelOptions(ctx.GetRequiredService<IContractResolver>(), path, isOptional));

            return builder;
        }
    }
}
