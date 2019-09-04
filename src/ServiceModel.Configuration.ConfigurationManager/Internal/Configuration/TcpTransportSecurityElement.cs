//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System.Configuration;
    using System.ServiceModel.Channels;
    using System.Globalization;
    using System.Net;
    using System.Net.Security;
#if DESKTOP
    using System.Security.Authentication.ExtendedProtection.Configuration;
#endif
    using System.ServiceModel;
    using System.ServiceModel.Security;
    using System.ComponentModel;
    using System.Security.Authentication;

    public sealed partial class TcpTransportSecurityElement : ServiceModelConfigurationElement
    {
        // From TcpTransportSecurity
        internal const TcpClientCredentialType DefaultClientCredentialType = TcpClientCredentialType.Windows;
        internal const ProtectionLevel DefaultProtectionLevel = ProtectionLevel.EncryptAndSign;

        [ConfigurationProperty(ConfigurationStrings.ClientCredentialType, DefaultValue = DefaultClientCredentialType)]
        [ServiceModelEnumValidator(typeof(TcpClientCredentialTypeHelper))]
        public TcpClientCredentialType ClientCredentialType
        {
            get { return (TcpClientCredentialType)base[ConfigurationStrings.ClientCredentialType]; }
            set { base[ConfigurationStrings.ClientCredentialType] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.ProtectionLevel, DefaultValue = DefaultProtectionLevel)]
        [ServiceModelEnumValidator(typeof(ProtectionLevelHelper))]
        public ProtectionLevel ProtectionLevel
        {
            get { return (ProtectionLevel)base[ConfigurationStrings.ProtectionLevel]; }
            set { base[ConfigurationStrings.ProtectionLevel] = value; }
        }

#if DESKTOP
        [ConfigurationProperty(ConfigurationStrings.ExtendedProtectionPolicy)]
        public ExtendedProtectionPolicyElement ExtendedProtectionPolicy
        {
            get { return (ExtendedProtectionPolicyElement)base[ConfigurationStrings.ExtendedProtectionPolicy]; }
            private set { base[ConfigurationStrings.ExtendedProtectionPolicy] = value; }
        }
#endif

        [ConfigurationProperty(ConfigurationStrings.SslProtocols, DefaultValue = TransportDefaults.OldDefaultSslProtocols)]
        [ServiceModelEnumValidator(typeof(SslProtocolsHelper))]
        public SslProtocols SslProtocols
        {
            get { return (SslProtocols)base[ConfigurationStrings.SslProtocols]; }
            private set { base[ConfigurationStrings.SslProtocols] = value; }
        }

        internal void ApplyConfiguration(TcpTransportSecurity security)
        {
            if (security == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("security");
            }
            security.ClientCredentialType = this.ClientCredentialType;
#if DESKTOP
            security.ProtectionLevel = this.ProtectionLevel;
            security.ExtendedProtectionPolicy = ChannelBindingUtility.BuildPolicy(this.ExtendedProtectionPolicy);
#endif
            security.SslProtocols = this.SslProtocols;
        }

        internal void InitializeFrom(TcpTransportSecurity security)
        {
            if (security == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("security");
            }
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.ClientCredentialType, security.ClientCredentialType);
#if DESKTOP
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.ProtectionLevel, security.ProtectionLevel);
            ChannelBindingUtility.InitializeFrom(security.ExtendedProtectionPolicy, this.ExtendedProtectionPolicy);
#endif
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.SslProtocols, security.SslProtocols);
        }
    }
}
