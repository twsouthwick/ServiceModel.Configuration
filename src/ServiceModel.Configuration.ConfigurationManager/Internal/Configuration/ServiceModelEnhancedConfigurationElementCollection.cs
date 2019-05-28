//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Globalization;
    using System.ServiceModel.Diagnostics;

    public abstract class ServiceModelEnhancedConfigurationElementCollection<TConfigurationElement> : ServiceModelConfigurationElementCollection<TConfigurationElement>
        where TConfigurationElement : ConfigurationElement, new()
    {
        internal ServiceModelEnhancedConfigurationElementCollection(string elementName)
            : base(ConfigurationElementCollectionType.AddRemoveClearMap, elementName)
        {
            this.AddElementName = elementName;
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            if (null == element)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("element");
            }

            // Is this a duplicate key?
            object newElementKey = this.GetElementKey(element);
            if (this.ContainsKey(newElementKey))
            {
                ConfigurationElement oldElement = this.BaseGet(newElementKey);
                if (null != oldElement)
                {
                    // Is oldElement present in the current level of config
                    // being manipulated (i.e. duplicate in same config file)
                    if (oldElement.ElementInformation.IsPresent)
                    {
                        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ConfigurationErrorsException(
                            SR.GetString(SR.ConfigDuplicateKeyAtSameScope, this.ElementName, newElementKey)));
                    }
                }
            }
            base.BaseAdd(element);
        }

        protected override bool ThrowOnDuplicate
        {
            get { return false; }
        }
    }
}
