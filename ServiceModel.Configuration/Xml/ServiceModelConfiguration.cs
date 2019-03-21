using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;
using System.Xml.Linq;

namespace ServiceModel.Configuration.Xml
{
    public class ServiceModelConfiguration
    {
        public static ServiceModelConfiguration Parse(XmlReader reader)
        {
            var doc = XDocument.Load(reader, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);

            var services = doc.Descendants("service")
                .Select(s => new Service
                {
                    Name = s.Attribute("name").Value,
                    Endpoints = s.Descendants("endpoint").Select(e => new Endpoint
                    {
                        Address = e.Attribute("address")?.Value ?? string.Empty,
                        BehaviorConfiguration = e.Attribute("behaviorConfiguration")?.Value ?? string.Empty,
                        Binding = e.Attribute("binding")?.Value ?? string.Empty,
                        Contract = e.Attribute("contract")?.Value ?? string.Empty,
                    }).ToList()
                })
                .ToList();

            return new ServiceModelConfiguration(services, null, null, null, null);
        }

        public ServiceModelConfiguration(
            IReadOnlyCollection<Service> services,
            IReadOnlyCollection<Binding> bindings,
            IReadOnlyCollection<IContractBehavior> contractBehaviors,
            IReadOnlyCollection<IEndpointBehavior> endpointBehaviors,
            IReadOnlyCollection<IOperationBehavior> operationBehaviors
            )
        {
            Services = services ?? Array.Empty<Service>();
            Bindings = bindings ?? Array.Empty<Binding>();
            ContractBehaviors = contractBehaviors ?? Array.Empty<IContractBehavior>();
            EndpointBehaviors = endpointBehaviors ?? Array.Empty<IEndpointBehavior>();
            OperationBehaviors = operationBehaviors ?? Array.Empty<IOperationBehavior>();
        }

        public IReadOnlyCollection<Service> Services { get; }

        public IReadOnlyCollection<Binding> Bindings { get; }

        public IReadOnlyCollection<IContractBehavior> ContractBehaviors { get; }

        public IReadOnlyCollection<IEndpointBehavior> EndpointBehaviors { get; }

        public IReadOnlyCollection<IOperationBehavior> OperationBehaviors { get; }
    }

    public class Service
    {
        public string Name { get; set; }

        public IEnumerable<Endpoint> Endpoints { get; set; }
    }

    public class Endpoint
    {
        public string Address { get; set; }

        public string BehaviorConfiguration { get; set; }

        public string Binding { get; set; }

        public string Contract { get; set; }
    }
}
