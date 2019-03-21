using System.ServiceModel;
using System.ServiceModel.Description;

namespace ServiceModel.Configuration
{
    internal class ConfiguredChannelFactory<T> : ChannelFactory<T>
    {
        public ConfiguredChannelFactory(ServiceEndpoint endpoint)
            : base(typeof(T))
        {
            InitializeEndpoint(endpoint);
        }
    }
}
