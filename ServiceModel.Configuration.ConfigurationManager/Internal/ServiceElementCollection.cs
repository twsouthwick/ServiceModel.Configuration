//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Globalization;

    [ConfigurationCollection(typeof(ServiceElement), AddItemName = ConfigurationStrings.Service)]
    internal sealed class ServiceElementCollection : ServiceModelEnhancedConfigurationElementCollection<ServiceElement>
    {
        public ServiceElementCollection()
            : base(ConfigurationStrings.Service)
        { }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            ServiceElement configElementKey = (ServiceElement)element;
            return configElementKey.Name;
        }
    }
}


