using Microsoft.Extensions.DependencyInjection;
using System;
using System.ServiceModel.Description;

namespace ServiceModel.Configuration
{
    public class ServiceModelBuilder
    {
        public ServiceModelBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public ServiceModelBuilder AddServiceEndpoint(Action<ServiceModelOptions> configure)
        {
            return AddServiceEndpoint(ServiceModelDefaults.DefaultName, configure);
        }

        public ServiceModelBuilder AddServiceEndpoint(string name, Action<ServiceModelOptions> configure)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Services.Configure(name, configure);

            return this;
        }

        public ServiceModelBuilder AddGlobalBehavior(IEndpointBehavior behavior)
        {
            Services.ConfigureAll<ServiceModelOptions>(option =>
            {
                foreach (var service in option.Services)
                {
                    service.Behaviors.Add(behavior);
                }
            });

            return this;
        }

        public ServiceModelBuilder AddDefaultChannel<T>(string name)
            where T : class
        {
            Services.AddSingleton(ctx => ctx.GetRequiredService<IChannelFactoryProvider>().CreateChannelFactory<T>(name).CreateChannel());

            return this;
        }
    }
}
