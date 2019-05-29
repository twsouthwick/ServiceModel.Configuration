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
    using System.ServiceModel;
    using System.ServiceModel.Security;
    using System.ComponentModel;

    public sealed partial class MessageSecurityOverTcpElement : ServiceModelConfigurationElement
    {
        internal const MessageCredentialType DefaultClientCredentialType = MessageCredentialType.Windows;

        [ConfigurationProperty(ConfigurationStrings.ClientCredentialType, DefaultValue = DefaultClientCredentialType)]
        [ServiceModelEnumValidator(typeof(MessageCredentialTypeHelper))]
        public MessageCredentialType ClientCredentialType
        {
            get { return (MessageCredentialType)base[ConfigurationStrings.ClientCredentialType]; }
            set { base[ConfigurationStrings.ClientCredentialType] = value; }
        }

#if DESKTOP
        [ConfigurationProperty(ConfigurationStrings.AlgorithmSuite, DefaultValue = ConfigurationStrings.Default)]
        [TypeConverter(typeof(SecurityAlgorithmSuiteConverter))]
        public SecurityAlgorithmSuite AlgorithmSuite
        {
            get { return (SecurityAlgorithmSuite)base[ConfigurationStrings.AlgorithmSuite]; }
            set { base[ConfigurationStrings.AlgorithmSuite] = value; }
        }
#endif

        internal void ApplyConfiguration(MessageSecurityOverTcp security)
        {
            if (security == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("security");
            }
            security.ClientCredentialType = this.ClientCredentialType;
#if DESKTOP
            if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.AlgorithmSuite].ValueOrigin)
            {
                security.AlgorithmSuite = this.AlgorithmSuite;
            }
#endif
        }

        internal void InitializeFrom(MessageSecurityOverTcp security)
        {
            if (security == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("security");
            }
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.ClientCredentialType, security.ClientCredentialType);
#if DESKTOP
            if (security.WasAlgorithmSuiteSet)
            {
                SetPropertyValueIfNotDefaultValue(ConfigurationStrings.AlgorithmSuite, security.AlgorithmSuite);
            }
#endif
        }
    }
}
