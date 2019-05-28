//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System.Configuration;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    public partial class BasicHttpBindingElement : HttpBindingBaseElement
    {
        public BasicHttpBindingElement(string name)
            : base(name)
        {
        }

        public BasicHttpBindingElement()
            : this(null)
        {
        }

        protected override Type BindingElementType
        {
            get { return typeof(BasicHttpBinding); }
        }

#if DESKTOP
        [ConfigurationProperty(ConfigurationStrings.MessageEncoding, DefaultValue = BasicHttpBindingDefaults.MessageEncoding)]
        [ServiceModelEnumValidator(typeof(WSMessageEncodingHelper))]
        public WSMessageEncoding MessageEncoding
        {
            get { return (WSMessageEncoding)base[ConfigurationStrings.MessageEncoding]; }
            set { base[ConfigurationStrings.MessageEncoding] = value; }
        }
#endif

        [ConfigurationProperty(ConfigurationStrings.Security)]
        public BasicHttpSecurityElement Security
        {
            get { return (BasicHttpSecurityElement)base[ConfigurationStrings.Security]; }
        }

        protected internal override void InitializeFrom(Binding binding)
        {
            base.InitializeFrom(binding);
            BasicHttpBinding bpBinding = (BasicHttpBinding)binding;

#if DESKTOP
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.MessageEncoding, bpBinding.MessageEncoding);
#endif
            this.Security.InitializeFrom(bpBinding.Security);
        }

        protected override void OnApplyConfiguration(Binding binding)
        {
            base.OnApplyConfiguration(binding);
            BasicHttpBinding bpBinding = (BasicHttpBinding)binding;
#if DESKTOP
            bpBinding.MessageEncoding = this.MessageEncoding;
#endif
            this.Security.ApplyConfiguration(bpBinding.Security);
        }
    }
}
