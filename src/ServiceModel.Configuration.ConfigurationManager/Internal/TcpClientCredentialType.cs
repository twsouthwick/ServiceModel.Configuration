//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------
namespace System.ServiceModel
{
    using System.Net;

    static class TcpClientCredentialTypeHelper
    {
        internal static bool IsDefined(TcpClientCredentialType value)
        {
            return (value == TcpClientCredentialType.None ||
                value == TcpClientCredentialType.Windows ||
                value == TcpClientCredentialType.Certificate);
        }
    }
}
