using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace ServiceModel.Configuration
{
    internal class ChannelFactoryProvider : IChannelFactoryProvider
    {
        private readonly IOptionsMonitor<ServiceModelOptions> _options;

        public ChannelFactoryProvider(IOptionsMonitor<ServiceModelOptions> options)
        {
            _options = options;
        }

        public ServiceEndpoint GetEndpoint<T>(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (_options.Get(name).Services.TryGet<T>(out var service))
            {
                var endpoint = service.ToServiceEndpoint(name);

                return endpoint;
            }

            throw new ServiceConfigurationException($"No service for {typeof(T)} is available for {name}");
        }

        public ChannelFactory<T> CreateChannelFactory<T>(string name) => new ConfiguredChannelFactory<T>(GetEndpoint<T>(name));
    }
}
