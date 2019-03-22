using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;

namespace ServiceModel.Configuration.Xml
{
     internal class XmlServiceModelOptions : IConfigureNamedOptions<ServiceModelOptions>
    {
        private readonly ITypeMapper _mapper;
        private readonly ServiceModelConfiguration _configuration;

        public XmlServiceModelOptions(ITypeMapper mapper, ServiceModelConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
        }

        public void Configure(string name, ServiceModelOptions options)
        {
            foreach (var service in _configuration.Services)
            {
                if (string.Equals(service.Name, name, StringComparison.Ordinal))
                {
                    foreach (var endpoint in service.Endpoints)
                    {
                        options.Services.Add(_mapper.GetType(endpoint.Contract), new ServiceModelService
                        {
                            Endpoint = new EndpointAddress(endpoint.Address)
                        });
                    }
                }
            }
        }

        public void Configure(ServiceModelOptions options)
        {
        }
    }
}
