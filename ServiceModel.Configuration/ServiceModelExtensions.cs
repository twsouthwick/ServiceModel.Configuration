using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace ServiceModel.Configuration
{
    public static class ServiceModelExtensions
    {
        public static ServiceModelBuilder AddServiceModelClient(this IServiceCollection services)
        {
            services.AddOptions<ServiceEndpoint>();
            services.AddTransient<IPostConfigureOptions<ServiceModelOptions>, ServiceEndpointValidation>();
            services.AddSingleton<IChannelFactoryProvider>(ctx => ctx.GetRequiredService<ChannelFactoryProvider>());
            services.AddSingleton<ChannelFactoryProvider>();
            services.AddSingleton(typeof(ChannelFactory<>), typeof(ConfiguredChannelFactory<>));

            services.AddSingleton<IContractResolver, DefaultContractResolver>();

            return new ServiceModelBuilder(services);
        }
    }
}
