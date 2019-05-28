//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System.Configuration;
    using System.ServiceModel;

    public sealed partial class BasicHttpsSecurityElement : ServiceModelConfigurationElement
    {
        private const BasicHttpsSecurityMode DefaultMode = BasicHttpsSecurityMode.Transport;

        [ConfigurationProperty(ConfigurationStrings.Mode, DefaultValue = DefaultMode)]
        [ServiceModelEnumValidator(typeof(BasicHttpsSecurityModeHelper))]
        public BasicHttpsSecurityMode Mode
        {
            get { return (BasicHttpsSecurityMode)base[ConfigurationStrings.Mode]; }
            set { base[ConfigurationStrings.Mode] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.Transport)]
        public HttpTransportSecurityElement Transport
        {
            get { return (HttpTransportSecurityElement)base[ConfigurationStrings.Transport]; }
        }

        [ConfigurationProperty(ConfigurationStrings.Message)]
        public BasicHttpMessageSecurityElement Message
        {
            get { return (BasicHttpMessageSecurityElement)base[ConfigurationStrings.Message]; }
        }

        internal void ApplyConfiguration(BasicHttpsSecurity security)
        {
            if (security == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("security");
            }

            security.Mode = this.Mode;
            this.Transport.ApplyConfiguration(security.Transport);
#if DESKTOP
            this.Message.ApplyConfiguration(security.Message);
#endif
        }

        internal void InitializeFrom(BasicHttpsSecurity security)
        {
            if (security == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("security");
            }

            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.Mode, security.Mode);
            this.Transport.InitializeFrom(security.Transport);

#if DESKTOP
            this.Message.InitializeFrom(security.Message);
#endif
        }
    }
}
