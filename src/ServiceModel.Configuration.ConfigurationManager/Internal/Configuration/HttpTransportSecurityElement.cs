//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.ServiceModel.Channels;
    using System.Globalization;
    using System.Net;
    using System.Net.Security;
    using System.Runtime;
    using System.ServiceModel;
    using System.ServiceModel.Security;
    using System.ComponentModel;

    public sealed partial class HttpTransportSecurityElement : ServiceModelConfigurationElement
    {
        [ConfigurationProperty(ConfigurationStrings.ClientCredentialType, DefaultValue = HttpClientCredentialType.None)]
        [ServiceModelEnumValidator(typeof(HttpClientCredentialTypeHelper))]
        public HttpClientCredentialType ClientCredentialType
        {
            get { return (HttpClientCredentialType)base[ConfigurationStrings.ClientCredentialType]; }
            set { base[ConfigurationStrings.ClientCredentialType] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.ProxyCredentialType, DefaultValue = HttpProxyCredentialType.None)]
        [ServiceModelEnumValidator(typeof(HttpProxyCredentialTypeHelper))]
        public HttpProxyCredentialType ProxyCredentialType
        {
            get { return (HttpProxyCredentialType)base[ConfigurationStrings.ProxyCredentialType]; }
            set { base[ConfigurationStrings.ProxyCredentialType] = value; }
        }

#if EXTENDED_PROTECTION_POLICY
        [ConfigurationProperty(ConfigurationStrings.ExtendedProtectionPolicy)]
        public ExtendedProtectionPolicyElement ExtendedProtectionPolicy
        {
            get { return (ExtendedProtectionPolicyElement)base[ConfigurationStrings.ExtendedProtectionPolicy]; }
            private set { base[ConfigurationStrings.ExtendedProtectionPolicy] = value; }
        }
#endif

#if REALM
        [ConfigurationProperty(ConfigurationStrings.Realm, DefaultValue = "")]
        [StringValidator(MinLength = 0)]
        public string Realm
        {
            get { return (string)base[ConfigurationStrings.Realm]; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    value = String.Empty;
                }
                base[ConfigurationStrings.Realm] = value;
            }
        }
#endif

        internal void ApplyConfiguration(HttpTransportSecurity security)
        {
            if (security == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("security");
            }
            security.ClientCredentialType = this.ClientCredentialType;
            security.ProxyCredentialType = this.ProxyCredentialType;
#if REALM
            security.Realm = this.Realm;
#endif

#if EXTENDED_PROTECTION_POLICY
            security.ExtendedProtectionPolicy = ChannelBindingUtility.BuildPolicy(this.ExtendedProtectionPolicy);
#endif
        }

        internal void InitializeFrom(HttpTransportSecurity security)
        {
            if (security == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("security");
            }
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.ClientCredentialType, security.ClientCredentialType);
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.ProxyCredentialType, security.ProxyCredentialType);
#if REALM
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.Realm, security.Realm);
#endif

#if EXTENDED_PROTECTION_POLICY
            ChannelBindingUtility.InitializeFrom(security.ExtendedProtectionPolicy, this.ExtendedProtectionPolicy);
#endif
        }
    }
}
