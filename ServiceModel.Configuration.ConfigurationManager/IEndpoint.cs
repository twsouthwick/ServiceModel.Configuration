using System.ServiceModel.Configuration;

namespace System
{
    internal interface IEndpoint : IConfigurationContextProviderInternal
    {
        string Contract { get; }

        string Binding { get; }

        string BindingConfiguration { get; }

        Uri Address { get; }
    }
}
