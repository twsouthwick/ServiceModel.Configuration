using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;

namespace ServiceModel.Configuration.Xml
{
    internal class XmlServiceModelOptions : IConfigureNamedOptions<ServiceModelOptions>
    {
        private readonly IContractResolver _mapper;
        private readonly ServiceModelConfiguration _configuration;

        public XmlServiceModelOptions(IContractResolver mapper, ServiceModelConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
        }

        public void Configure(string name, ServiceModelOptions options)
        {
            if (_configuration.Services.TryGetValue(name, out var service))
            {
                foreach (var endpoint in service.Endpoints)
                {
                    if (_mapper.TryResolve(endpoint.Contract, out var contract))
                    {
                        options.Services.Add(contract, new ServiceModelService
                        {
                            Endpoint = new EndpointAddress(endpoint.Address)
                        });
                    }
                    else
                    {
                        throw new ServiceConfigurationException($"Could not resolve contract type '{endpoint.Contract}'");
                    }
                }
            }
        }

        public void Configure(ServiceModelOptions options)
        {
        }
    }
}
