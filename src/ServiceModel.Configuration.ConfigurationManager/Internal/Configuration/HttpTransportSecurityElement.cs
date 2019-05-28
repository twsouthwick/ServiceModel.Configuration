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
#if DESKTOP
    using System.Security.Authentication.ExtendedProtection.Configuration;
#endif
    using System.ServiceModel;
    using System.ServiceModel.Security;
    using System.ComponentModel;

    public sealed partial class HttpTransportSecurityElement : ServiceModelConfigurationElement
    {
        private const HttpClientCredentialType DefaultClientCredentialType = HttpClientCredentialType.None;
        private const HttpProxyCredentialType DefaultProxyCredentialType = HttpProxyCredentialType.None;
        private const string DefaultRealm = "";

        [ConfigurationProperty(ConfigurationStrings.ClientCredentialType, DefaultValue = DefaultClientCredentialType)]
        [ServiceModelEnumValidator(typeof(HttpClientCredentialTypeHelper))]
        public HttpClientCredentialType ClientCredentialType
        {
            get { return (HttpClientCredentialType)base[ConfigurationStrings.ClientCredentialType]; }
            set { base[ConfigurationStrings.ClientCredentialType] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.ProxyCredentialType, DefaultValue = DefaultProxyCredentialType)]
        [ServiceModelEnumValidator(typeof(HttpProxyCredentialTypeHelper))]
        public HttpProxyCredentialType ProxyCredentialType
        {
            get { return (HttpProxyCredentialType)base[ConfigurationStrings.ProxyCredentialType]; }
            set { base[ConfigurationStrings.ProxyCredentialType] = value; }
        }

#if DESKTOP
        [ConfigurationProperty(ConfigurationStrings.ExtendedProtectionPolicy)]
        public ExtendedProtectionPolicyElement ExtendedProtectionPolicy
        {
            get { return (ExtendedProtectionPolicyElement)base[ConfigurationStrings.ExtendedProtectionPolicy]; }
            private set { base[ConfigurationStrings.ExtendedProtectionPolicy] = value; }
        }
#endif

        [ConfigurationProperty(ConfigurationStrings.Realm, DefaultValue = DefaultRealm)]
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

        internal void ApplyConfiguration(HttpTransportSecurity security)
        {
            if (security == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("security");
            }
            security.ClientCredentialType = this.ClientCredentialType;
            security.ProxyCredentialType = this.ProxyCredentialType;
#if DESKTOP
            security.Realm = this.Realm;
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

#if DESKTOP
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.Realm, security.Realm);

            ChannelBindingUtility.InitializeFrom(security.ExtendedProtectionPolicy, this.ExtendedProtectionPolicy);
#endif
        }
    }
}
