//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System.Configuration;
using System.ServiceModel.Channels;

    public abstract partial class TransportElement : BindingElementExtensionElement
    {
        protected TransportElement()
        {
        }

        public override void ApplyConfiguration(BindingElement bindingElement)
        {
            base.ApplyConfiguration(bindingElement);
            TransportBindingElement binding = (TransportBindingElement)bindingElement;
            binding.ManualAddressing = this.ManualAddressing;
#if DESKTOP
            binding.MaxBufferPoolSize = this.MaxBufferPoolSize;
#endif
            binding.MaxReceivedMessageSize = this.MaxReceivedMessageSize;
        }

        public override void CopyFrom(ServiceModelExtensionElement from)
        {
            base.CopyFrom(from);

            TransportElement source = (TransportElement)from;
            this.ManualAddressing = source.ManualAddressing;
            this.MaxBufferPoolSize = source.MaxBufferPoolSize;
            this.MaxReceivedMessageSize = source.MaxReceivedMessageSize;
        }

        protected internal override BindingElement CreateBindingElement()
        {
            TransportBindingElement binding = this.CreateDefaultBindingElement();
            this.ApplyConfiguration(binding);
            return binding;
        }

        protected abstract TransportBindingElement CreateDefaultBindingElement();

        protected internal override void InitializeFrom(BindingElement bindingElement)
        {
            base.InitializeFrom(bindingElement);
            TransportBindingElement binding = (TransportBindingElement)bindingElement;
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.ManualAddressing, binding.ManualAddressing);
#if DESKTOP
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.MaxBufferPoolSize, binding.MaxBufferPoolSize);
#endif
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.MaxReceivedMessageSize, binding.MaxReceivedMessageSize);
        }

        [ConfigurationProperty(ConfigurationStrings.ManualAddressing, DefaultValue = false)]
        public bool ManualAddressing
        {
            get { return (bool)base[ConfigurationStrings.ManualAddressing]; }
            set { base[ConfigurationStrings.ManualAddressing] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.MaxBufferPoolSize, DefaultValue = TransportDefaults.MaxBufferPoolSize)]
        [LongValidator(MinValue = 1)]
        public long MaxBufferPoolSize
        {
            get { return (long)base[ConfigurationStrings.MaxBufferPoolSize]; }
            set { base[ConfigurationStrings.MaxBufferPoolSize] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.MaxReceivedMessageSize, DefaultValue = TransportDefaults.MaxReceivedMessageSize)]
        [LongValidator(MinValue = 1)]
        public long MaxReceivedMessageSize
        {
            get { return (long)base[ConfigurationStrings.MaxReceivedMessageSize]; }
            set { base[ConfigurationStrings.MaxReceivedMessageSize] = value; }
        }
    }
}



