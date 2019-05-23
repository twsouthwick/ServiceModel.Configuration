using Microsoft.Extensions.Options;
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

        public void Configure(string _, ServiceModelOptions options) => Configure(options);

        public void Configure(ServiceModelOptions options)
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
                    Configure(options, group);
                }
                else
                {
                    throw new ServiceModelConfigurationException("Not valid type");
                }
            }
        }

        private void Configure(ServiceModelOptions options, ServiceModelSectionGroup group)
        {
            foreach (var service in group.Services.Services.Cast<ServiceElement>())
            {
                foreach (var endpoint in service.Endpoints.Cast<ServiceEndpointElement>())
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
}
