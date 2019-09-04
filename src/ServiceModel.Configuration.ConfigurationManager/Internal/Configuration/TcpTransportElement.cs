//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System.Configuration;
    using System.Net.Sockets;
    using System.ServiceModel.Channels;

    public sealed partial class TcpTransportElement : ConnectionOrientedTransportElement
    {
        public TcpTransportElement()
            : base()
        {
        }

        public override void ApplyConfiguration(BindingElement bindingElement)
        {
            base.ApplyConfiguration(bindingElement);
            TcpTransportBindingElement binding = (TcpTransportBindingElement)bindingElement;
            PropertyInformationCollection propertyInfo = this.ElementInformation.Properties;

#if DESKTOP
            if (this.ListenBacklog != TcpTransportDefaults.ListenBacklogConst)
            {
                binding.ListenBacklog = this.ListenBacklog;
            }
            binding.PortSharingEnabled = this.PortSharingEnabled;
            binding.TeredoEnabled = this.TeredoEnabled;
#endif
            this.ConnectionPoolSettings.ApplyConfiguration(binding.ConnectionPoolSettings);

#if DESKTOP
            binding.ExtendedProtectionPolicy = ChannelBindingUtility.BuildPolicy(this.ExtendedProtectionPolicy);
#endif
        }

        public override Type BindingElementType
        {
            get { return typeof(TcpTransportBindingElement); }
        }

        public override void CopyFrom(ServiceModelExtensionElement from)
        {
            base.CopyFrom(from);

            TcpTransportElement source = (TcpTransportElement)from;
            this.ListenBacklog = source.ListenBacklog;
            this.PortSharingEnabled = source.PortSharingEnabled;
            this.TeredoEnabled = source.TeredoEnabled;
            this.ConnectionPoolSettings.CopyFrom(source.ConnectionPoolSettings);
#if DESKTOP
            ChannelBindingUtility.CopyFrom(source.ExtendedProtectionPolicy, this.ExtendedProtectionPolicy);
#endif
        }

        protected override TransportBindingElement CreateDefaultBindingElement()
        {
            return new TcpTransportBindingElement();
        }

        protected internal override void InitializeFrom(BindingElement bindingElement)
        {
            base.InitializeFrom(bindingElement);
            TcpTransportBindingElement binding = (TcpTransportBindingElement)bindingElement;
#if DESKTOP
            if (binding.IsListenBacklogSet)
            {
                ConfigurationProperty listenBacklogProperty = this.Properties[ConfigurationStrings.ListenBacklog];
                SetPropertyValue(listenBacklogProperty, binding.ListenBacklog, false /*ignore locks*/);
            }
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.PortSharingEnabled, binding.PortSharingEnabled);
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.TeredoEnabled, binding.TeredoEnabled);
#endif
            this.ConnectionPoolSettings.InitializeFrom(binding.ConnectionPoolSettings);
#if DESKTOP
            ChannelBindingUtility.InitializeFrom(binding.ExtendedProtectionPolicy, this.ExtendedProtectionPolicy);
#endif
        }

        [ConfigurationProperty(ConfigurationStrings.ListenBacklog, DefaultValue = TcpTransportDefaults.ListenBacklogConst)]
        [IntegerValidator(MinValue = 0)]
        public int ListenBacklog
        {
            get { return (int)base[ConfigurationStrings.ListenBacklog]; }
            set { base[ConfigurationStrings.ListenBacklog] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.PortSharingEnabled, DefaultValue = TcpTransportDefaults.PortSharingEnabled)]
        public bool PortSharingEnabled
        {
            get { return (bool)base[ConfigurationStrings.PortSharingEnabled]; }
            set { base[ConfigurationStrings.PortSharingEnabled] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.TeredoEnabled, DefaultValue = TcpTransportDefaults.TeredoEnabled)]
        public bool TeredoEnabled
        {
            get { return (bool)base[ConfigurationStrings.TeredoEnabled]; }
            set { base[ConfigurationStrings.TeredoEnabled] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.ConnectionPoolSettings)]
        public TcpConnectionPoolSettingsElement ConnectionPoolSettings
        {
            get { return (TcpConnectionPoolSettingsElement)base[ConfigurationStrings.ConnectionPoolSettings]; }
            set { base[ConfigurationStrings.ConnectionPoolSettings] = value; }
        }

#if DESKTOP
        [ConfigurationProperty(ConfigurationStrings.ExtendedProtectionPolicy)]
        public ExtendedProtectionPolicyElement ExtendedProtectionPolicy
        {
            get { return (ExtendedProtectionPolicyElement)base[ConfigurationStrings.ExtendedProtectionPolicy]; }
            private set { base[ConfigurationStrings.ExtendedProtectionPolicy] = value; }
        }
#endif
    }
}



