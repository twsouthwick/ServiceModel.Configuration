using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ServiceModel.Description;

namespace ServiceModel.Configuration
{
    public static class ServiceModelExtensions
    {
        public static ServiceModelBuilder AddServiceModelClient(this IServiceCollection services)
        {
            services.AddOptions<ServiceEndpoint>();
            services.AddTransient<IPostConfigureOptions<ServiceModelOptions>, ServiceEndpointValidation>();
            services.AddSingleton<IChannelFactoryProvider, ChannelFactoryProvider>();

            return new ServiceModelBuilder(services);
        }
    }
}
