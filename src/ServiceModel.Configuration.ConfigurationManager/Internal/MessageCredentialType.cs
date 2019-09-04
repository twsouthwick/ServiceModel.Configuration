//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------
namespace System.ServiceModel
{
    static class MessageCredentialTypeHelper
    {
        internal static bool IsDefined(MessageCredentialType value)
        {
            return (value == MessageCredentialType.None ||
                value == MessageCredentialType.UserName ||
                value == MessageCredentialType.Windows ||
                value == MessageCredentialType.Certificate ||
                value == MessageCredentialType.IssuedToken);
        }
    }
}
