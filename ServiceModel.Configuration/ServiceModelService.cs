using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace ServiceModel.Configuration
{
    public class ServiceModelService
    {
        public EndpointAddress Endpoint { get; set; }

        public Binding Binding { get; set; }

        public ICollection<IEndpointBehavior> Behaviors { get; } = new List<IEndpointBehavior>();

        internal ServiceEndpoint ToServiceEndpoint(string name)
        {
            var endpoint = new ServiceEndpoint(new ContractDescription(name), Binding, Endpoint);

            foreach (var behavior in Behaviors)
            {
                endpoint.EndpointBehaviors.Add(behavior);
            }

            return endpoint;
        }
    }
}
