//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System.ComponentModel;
    using System.Configuration;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Text;

    public partial class BasicHttpsBindingElement : HttpBindingBaseElement
    {
        public BasicHttpsBindingElement(string name)
            : base(name)
        {
        }

        public BasicHttpsBindingElement()
            : this(null)
        {
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
        public BasicHttpsSecurityElement Security
        {
            get { return (BasicHttpsSecurityElement)base[ConfigurationStrings.Security]; }
        }

        protected override Type BindingElementType
        {
            get { return typeof(BasicHttpsBinding); }
        }

        protected internal override void InitializeFrom(Binding binding)
        {
            base.InitializeFrom(binding);
            BasicHttpsBinding bpBinding = (BasicHttpsBinding)binding;

#if DESKTOP
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.MessageEncoding, bpBinding.MessageEncoding);
#endif
            this.Security.InitializeFrom(bpBinding.Security);
        }

        protected override void OnApplyConfiguration(Binding binding)
        {
            base.OnApplyConfiguration(binding);
            BasicHttpsBinding bpBinding = (BasicHttpsBinding)binding;

#if DESKTOP
            bpBinding.MessageEncoding = this.MessageEncoding;
#endif

            this.Security.ApplyConfiguration(bpBinding.Security);
        }
    }
}
