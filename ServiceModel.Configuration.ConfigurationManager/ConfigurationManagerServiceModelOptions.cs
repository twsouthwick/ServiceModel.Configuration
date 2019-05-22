using Microsoft.Extensions.Options;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Xml.Linq;

namespace ServiceModel.Configuration.Xml
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

        public void Configure(string name, ServiceModelOptions options)
        {
            var configuration = Load(_path);
            var section = ServiceModelSectionGroup.GetSectionGroup(configuration);

            if (section == null)
            {
                if (_isOptional)
                {
                    return;
                }

                throw new InvalidOperationException("Section not found");
            }

            if (section is ServiceModelSectionGroup group)
            {
                Configure(name, options, group);
            }
            else
            {
                throw new InvalidOperationException("Not valid type");
            }
        }

        private static System.Configuration.Configuration Load(string path)
        {
            return ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(path));
            //var tmp = Path.GetTempFileName();

            ////initial.SectionGroups.Add("system.serviceModel", new ServiceModelSectionGroup());

            //initial.SaveAs(tmp);

            //var final = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(tmp));

            //File.Delete(tmp);

            //return final;
        }

        private void Configure(string name, ServiceModelOptions options, ServiceModelSectionGroup group)
        {
            foreach (var service in group.Services.Services.Cast<ServiceElement>())
            {
                foreach (var endpoint in service.Endpoints.Cast<ServiceEndpointElement>())
                {
                    if (_mapper.TryResolve(endpoint.Contract, out var type))
                    {
                        options.Services.Add(type, o =>
                        {
                            o.Endpoint = new EndpointAddress(endpoint.Address);
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
