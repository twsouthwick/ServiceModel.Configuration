//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System.Configuration;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.Globalization;
    using System.ServiceModel;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public sealed partial class MetadataElement : ConfigurationElement
    {
        public MetadataElement()
        {
        }

#if DESKTOP
        [ConfigurationProperty(ConfigurationStrings.PolicyImporters)]
        public PolicyImporterElementCollection PolicyImporters
        {
            get { return (PolicyImporterElementCollection)base[ConfigurationStrings.PolicyImporters]; }
        }

        [ConfigurationProperty(ConfigurationStrings.WsdlImporters)]
        public WsdlImporterElementCollection WsdlImporters
        {
            get { return (WsdlImporterElementCollection)base[ConfigurationStrings.WsdlImporters]; }
        }

        public Collection<IWsdlImportExtension> LoadWsdlImportExtensions()
        {
            return ConfigLoader.LoadWsdlImporters(this.WsdlImporters, this.EvaluationContext);
        }

        public Collection<IPolicyImportExtension> LoadPolicyImportExtensions()
        {
            return ConfigLoader.LoadPolicyImporters(this.PolicyImporters, this.EvaluationContext);
        }
#endif

        internal void SetDefaults()
        {
#if DESKTOP
            this.PolicyImporters.SetDefaults();
            this.WsdlImporters.SetDefaults();
#endif
        }
    }
}



