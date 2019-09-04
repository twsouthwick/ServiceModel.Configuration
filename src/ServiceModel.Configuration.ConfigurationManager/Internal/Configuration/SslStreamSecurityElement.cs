//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System.Configuration;
    using System.Security.Authentication;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Security;

    public sealed partial class SslStreamSecurityElement : BindingElementExtensionElement
    {
        public SslStreamSecurityElement()
        {
        }

        [ConfigurationProperty(
            ConfigurationStrings.RequireClientCertificate, DefaultValue = TransportDefaults.RequireClientCertificate)]
        public bool RequireClientCertificate
        {
            get { return (bool)base[ConfigurationStrings.RequireClientCertificate]; }
            set { base[ConfigurationStrings.RequireClientCertificate] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.SslProtocols, DefaultValue = TransportDefaults.OldDefaultSslProtocols)]
        [ServiceModelEnumValidator(typeof(SslProtocolsHelper))]
        public SslProtocols SslProtocols
        {
            get { return (SslProtocols)base[ConfigurationStrings.SslProtocols]; }
            private set { base[ConfigurationStrings.SslProtocols] = value; }
        }


        public override void ApplyConfiguration(BindingElement bindingElement)
        {
            base.ApplyConfiguration(bindingElement);
            SslStreamSecurityBindingElement sslBindingElement =
                (SslStreamSecurityBindingElement)bindingElement;
#if DESKTOP
            sslBindingElement.RequireClientCertificate = this.RequireClientCertificate;
#endif
            sslBindingElement.SslProtocols = this.SslProtocols;
        }

        protected internal override BindingElement CreateBindingElement()
        {
            SslStreamSecurityBindingElement sslBindingElement
                = new SslStreamSecurityBindingElement();

            this.ApplyConfiguration(sslBindingElement);
            return sslBindingElement;
        }

        public override Type BindingElementType
        {
            get { return typeof(SslStreamSecurityBindingElement); }
        }

        public override void CopyFrom(ServiceModelExtensionElement from)
        {
            base.CopyFrom(from);

            SslStreamSecurityElement source = (SslStreamSecurityElement)from;
            this.RequireClientCertificate = source.RequireClientCertificate;
            this.SslProtocols = source.SslProtocols;
        }

        protected internal override void InitializeFrom(BindingElement bindingElement)
        {
            base.InitializeFrom(bindingElement);
            SslStreamSecurityBindingElement sslBindingElement
                = (SslStreamSecurityBindingElement)bindingElement;
#if DESKTOP
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.RequireClientCertificate, sslBindingElement.RequireClientCertificate);
#endif
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.SslProtocols, sslBindingElement.SslProtocols);
        }
    }
}



