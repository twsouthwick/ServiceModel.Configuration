//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System.Configuration;
    using System.Runtime;
    using System.Security;

    public sealed partial class ServiceElement : ConfigurationElement, IConfigurationContextProviderInternal
    {
        [SecurityCritical]
        EvaluationContextHelper contextHelper;

        public ServiceElement() : base() { }

        public ServiceElement(string serviceName)
            : this()
        {
            this.Name = serviceName;
        }

        [ConfigurationProperty(ConfigurationStrings.BehaviorConfiguration, DefaultValue = "")]
        [StringValidator(MinLength = 0)]
        public string BehaviorConfiguration
        {
            get { return (string)base[ConfigurationStrings.BehaviorConfiguration]; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    value = String.Empty;
                }
                base[ConfigurationStrings.BehaviorConfiguration] = value;
            }
        }

        [ConfigurationProperty(ConfigurationStrings.DefaultCollectionName, Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public ServiceEndpointElementCollection Endpoints
        {
            get { return (ServiceEndpointElementCollection)base[ConfigurationStrings.DefaultCollectionName]; }
        }

        //[ConfigurationProperty(ConfigurationStrings.Host, Options = ConfigurationPropertyOptions.None)]
        //public HostElement Host
        //{
        //    get { return (HostElement)base[ConfigurationStrings.Host]; }
        //}

        [ConfigurationProperty(ConfigurationStrings.Name, Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired)]
        [StringValidator(MinLength = 1)]
        public string Name
        {
            get { return (string)base[ConfigurationStrings.Name]; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    value = String.Empty;
                }
                base[ConfigurationStrings.Name] = value;
            }
        }

        [SecurityCritical]
        protected override void Reset(ConfigurationElement parentElement)
        {
            this.contextHelper.OnReset(parentElement);

            base.Reset(parentElement);
        }

        ContextInformation IConfigurationContextProviderInternal.GetEvaluationContext()
        {
            return this.EvaluationContext;
        }

        [SecurityCritical]
        ContextInformation IConfigurationContextProviderInternal.GetOriginalEvaluationContext()
        {
            return this.contextHelper.GetOriginalContext(this);
        }
    }
}



