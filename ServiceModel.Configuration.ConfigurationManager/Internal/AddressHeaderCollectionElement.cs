//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System;
    using System.Configuration;
    using System.ServiceModel.Channels;
    using System.Xml;
    using System.Security;
    using System.Runtime;
    using System.Diagnostics;
    using System.Collections.Generic;

    public sealed partial class AddressHeaderCollectionElement : ServiceModelConfigurationElement
    {
        public AddressHeaderCollectionElement()
        {
        }

        internal void Copy(AddressHeaderCollectionElement source)
        {
            if (source == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("source");
            }

            PropertyInformationCollection properties = source.ElementInformation.Properties;
            if (properties[ConfigurationStrings.Headers].ValueOrigin != PropertyValueOrigin.Default)
            {
                this.Headers = source.Headers;
            }
        }

        [ConfigurationProperty(ConfigurationStrings.Headers, DefaultValue = null)]
        public AddressHeaderCollection Headers
        {
            get
            {
                AddressHeaderCollection retVal = (AddressHeaderCollection)base[ConfigurationStrings.Headers];
                if (null == retVal)
                {
                    retVal = new AddressHeaderCollection();
                }
                return retVal;
            }
            set
            {
                if (value == null)
                {
                    value = new AddressHeaderCollection();
                }
                base[ConfigurationStrings.Headers] = value;
            }
        }

        [SecuritySafeCritical]
        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            SetIsPresent();
            DeserializeElementCore(reader);
        }

        private void DeserializeElementCore(XmlReader reader)
        {
#if DESKTOP
            this.Headers = AddressHeaderCollection.ReadServiceParameters(XmlDictionaryReader.CreateDictionaryReader(reader));
#else
            throw new PlatformNotSupportedException();
#endif
        }

        [SecurityCritical]
        void SetIsPresent()
        {
            ConfigurationHelpers.SetIsPresent(this);
        }

        protected override bool SerializeToXmlElement(XmlWriter writer, String elementName)
        {
#if DESKTOP
            bool dataToWrite = this.Headers.Count != 0;
            if (dataToWrite && writer != null)
            {
                writer.WriteStartElement(elementName);
                this.Headers.WriteContentsTo(XmlDictionaryWriter.CreateDictionaryWriter(writer));
                writer.WriteEndElement();
            }
            return dataToWrite;
#else
            throw new PlatformNotSupportedException();
#endif
        }

        internal void InitializeFrom(AddressHeaderCollection headers)
        {
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.Headers, headers);
        }
    }
}



