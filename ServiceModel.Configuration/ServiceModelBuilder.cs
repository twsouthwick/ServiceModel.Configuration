using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.ServiceModel.Description;
using System.Xml;

namespace ServiceModel.Configuration
{
    public class ServiceModelBuilder
    {
        public ServiceModelBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public ServiceModelBuilder AddServiceEndpoint(string name, Action<ServiceModelOptions> configure)
        {
            Services.Configure(name, configure);

            return this;
        }

        public ServiceModelBuilder AddGlobalBehavior(IEndpointBehavior behavior)
        {
            Services.ConfigureAll<ServiceModelOptions>(option =>
            {
                option.Behaviors.Add(behavior);
            });

            return this;
        }
    }
}
