using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

namespace ServiceModel.Configuration
{
    internal class ConfigurationManagerServiceModelOptions : IConfigureNamedOptions<ServiceModelOptions>, IDisposable
    {
        private readonly IContractResolver _mapper;
        private readonly Lazy<ServiceModelSectionGroup> _section;
        private readonly WrappedConfigurationFile _file;

        public ConfigurationManagerServiceModelOptions(IContractResolver mapper, string path, bool isOptional)
        {
            _mapper = mapper;
            _file = new WrappedConfigurationFile(path);

            _section = new Lazy<ServiceModelSectionGroup>(() =>
            {
                var configuration = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(_file.ConfigPath));
                var section = ServiceModelSectionGroup.GetSectionGroup(configuration);

                if (section is null && !isOptional)
                {
                    throw new ServiceModelConfigurationException("Section not found");
                }

                return section;
            }, true);
        }

        public void Dispose() => _file.Dispose();

        public void Configure(ServiceModelOptions options) => Configure(ServiceModelDefaults.DefaultName, options);

        public void Configure(string name, ServiceModelOptions options)
        {
            Configure(name, options, _section.Value);
        }

        private void Configure(string name, ServiceModelOptions options, ServiceModelSectionGroup group)
        {
            if (group is null)
            {
                return;
            }

            if (string.Equals(ServiceModelDefaults.DefaultName, name, StringComparison.Ordinal))
            {
                Add(options, group.Client?.Endpoints);
            }
            else
            {
                var service = group.Services.Services.Cast<ServiceElement>().FirstOrDefault(e => e.Name == name);

                if (service != null)
                {
                    Add(options, service.Endpoints);
                }
            }
        }

        private void Add(ServiceModelOptions options, IEnumerable endpoints)
        {
            if (endpoints is null)
            {
                return;
            }

            foreach (var endpoint in endpoints.OfType<IEndpoint>())
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
