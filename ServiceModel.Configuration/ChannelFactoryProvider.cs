using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;

namespace ServiceModel.Configuration
{
    internal class ChannelFactoryProvider<T> : IChannelFactoryProvider<T>
    {
        private readonly IOptionsMonitor<ServiceModelOptions> _options;

        public ChannelFactoryProvider(IOptionsMonitor<ServiceModelOptions> options)
        {
            _options = options;
        }

        public ChannelFactory<T> CreateChannelFactory(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var endpoint = _options.Get(name).ToServiceEndpoint(name);

            return new ConfiguredChannelFactory<T>(endpoint);
        }
    }
}
