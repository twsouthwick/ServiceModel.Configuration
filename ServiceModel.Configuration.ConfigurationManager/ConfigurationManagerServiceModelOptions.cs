using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

namespace ServiceModel.Configuration
{
    internal class ConfigurationManagerServiceModelOptions : IConfigureNamedOptions<ServiceModelOptions>
    {
        private readonly IContractResolver _mapper;
        private readonly string _path;
        private readonly bool _isOptional;

        public ConfigurationManagerServiceModelOptions(IContractResolver mapper, string path, bool isOptional)
        {
            _mapper = mapper;
            _path = path;
            _isOptional = isOptional;
        }

        public void Configure(ServiceModelOptions options) => Configure(ServiceModelDefaults.DefaultName, options);

        public void Configure(string name, ServiceModelOptions options)
        {
            using (var configFile = new WrappedConfigurationFile(_path))
            {
                var configuration = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(configFile.ConfigPath));
                var section = ServiceModelSectionGroup.GetSectionGroup(configuration);

                if (section == null)
                {
                    if (_isOptional)
                    {
                        return;
                    }

                    throw new ServiceModelConfigurationException("Section not found");
                }

                if (section is ServiceModelSectionGroup group)
                {
                    Configure(name, options, group);
                }
                else
                {
                    throw new ServiceModelConfigurationException("Not valid type");
                }
            }
        }

        private void Configure(string name, ServiceModelOptions options, ServiceModelSectionGroup group)
        {
            var service = group.Services.Services.Cast<ServiceElement>().FirstOrDefault(e => e.Name == name);

            if (service != null)
            {
                Add(options, service.Endpoints.Cast<ServiceEndpointElement>());
            }
        }

        private void Add(ServiceModelOptions options, IEnumerable<ServiceEndpointElement> endpoints)
        {
            foreach (var endpoint in endpoints)
            {
                options.Services.Add(_mapper.ResolveContract(endpoint.Contract), o =>
                {
                    o.Endpoint = new EndpointAddress(endpoint.Address);

                    if (!string.IsNullOrEmpty(endpoint.Binding) || !string.IsNullOrEmpty(endpoint.BindingConfiguration))
                    {
                        o.Binding = ConfigLoader.LookupBinding(endpoint.Binding, endpoint.BindingConfiguration, ConfigurationHelpers.GetEvaluationContext(endpoint));
                    }
                });
            }
        }
    }
}
