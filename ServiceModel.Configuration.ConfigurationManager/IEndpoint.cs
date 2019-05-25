﻿using System.ServiceModel.Configuration;

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


namespace System.ServiceModel.Configuration
{
    partial class ChannelEndpointElement : IEndpoint
    {
    }

    partial class ServiceEndpointElement : IEndpoint
    {
    }
}