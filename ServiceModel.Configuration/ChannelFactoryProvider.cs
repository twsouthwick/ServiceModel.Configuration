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
                return CreateEndpoint<T>(service);
            }

            throw new ServiceConfigurationException($"No service for {typeof(T)} is available for {name}");
        }

        public ChannelFactory<T> CreateChannelFactory<T>(string name) => new ConfiguredChannelFactory<T>(GetEndpoint<T>(name));

        private static ServiceEndpoint CreateEndpoint<T>(ServiceModelService service)
        {
            var contract = ContractDescription.GetContract(typeof(T));
            var endpoint = new ServiceEndpoint(contract, service.Binding, service.Endpoint);

            foreach (var behavior in service.Behaviors)
            {
                endpoint.EndpointBehaviors.Add(behavior);
            }

            return endpoint;
        }
    }
}
