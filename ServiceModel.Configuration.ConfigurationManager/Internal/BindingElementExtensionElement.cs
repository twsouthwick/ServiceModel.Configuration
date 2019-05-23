//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------


namespace System.ServiceModel.Configuration
{
    using System;
    using System.ServiceModel.Channels;
    using System.Configuration;
    using System.Globalization;
    using System.Xml;

    public abstract class BindingElementExtensionElement : ServiceModelExtensionElement
    {
        public virtual void ApplyConfiguration(BindingElement bindingElement)
        {
            if (bindingElement == null)
            {
                throw new ArgumentNullException(nameof(bindingElement));
            }
        }

        public abstract Type BindingElementType
        {
            get;
        }

        protected internal abstract BindingElement CreateBindingElement();

        protected internal virtual void InitializeFrom(BindingElement bindingElement)
        {
            if (bindingElement == null)
            {
                throw new ArgumentNullException(nameof(bindingElement));
            }

            if (bindingElement.GetType() != this.BindingElementType)
            {
                throw new ArgumentException("bindingElement");//, SR.GetString(SR.ConfigInvalidTypeForBindingElement, this.BindingElementType.ToString(), bindingElement.GetType().ToString()));
            }
        }
    }
}
