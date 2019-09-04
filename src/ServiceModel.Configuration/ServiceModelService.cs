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
    }
}
