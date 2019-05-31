using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

namespace System
{
    internal interface IEndpoint : IConfigurationContextProviderInternal
    {
        string Contract { get; }

        string Binding { get; }

        string BindingConfiguration { get; }

        Uri Address { get; }
    }

    internal static class EndpointExtensions
    {
        public static EndpointAddress GetEndpoint(this IEndpoint endpoint)
        {
            if (endpoint is ChannelEndpointElement e)
            {
                return ConfigLoader.LookupEndpoint()
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}


namespace System.ServiceModel.Configuration
{
    partial class ChannelEndpointElement : IEndpoint
    {
    }

    partial class ServiceEndpointElement : IEndpoint
    {
    }
}