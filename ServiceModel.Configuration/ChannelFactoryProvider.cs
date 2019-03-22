using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;

namespace ServiceModel.Configuration
{
    internal class ChannelFactoryProvider : IChannelFactoryProvider
    {
        private readonly IOptionsMonitor<ServiceModelOptions> _options;

        public ChannelFactoryProvider(IOptionsMonitor<ServiceModelOptions> options)
        {
            _options = options;
        }

        public ChannelFactory<T> CreateChannelFactory<T>(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (_options.Get(name).Services.TryGet<T>(out var service))
            {
                var endpoint = service.ToServiceEndpoint(name);

                return new ConfiguredChannelFactory<T>(endpoint);
            }

            throw new ServiceConfigurationException($"No service for {typeof(T)} is available for {name}");
        }
    }
}
