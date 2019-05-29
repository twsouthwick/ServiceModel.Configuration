//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System;
    using System.Configuration;
    using System.IdentityModel.Selectors;
#if DESKTOP
    using System.IdentityModel.Tokens;
#endif
    using System.Security.Cryptography.X509Certificates;
    using System.ServiceModel;
    using System.ServiceModel.Security;
    using System.Xml;

    public sealed partial class IssuedTokenServiceElement : ConfigurationElement
    {
        private const bool DefaultAllowUntrustedRsaIssuers = false;
        private const X509CertificateValidationMode DefaultCertificateValidationMode = X509CertificateValidationMode.ChainTrust;
        private const X509RevocationMode DefaultRevocationMode = X509RevocationMode.Online;
        private const StoreLocation DefaultTrustedStoreLocation = StoreLocation.LocalMachine;

        public IssuedTokenServiceElement()
        {
        }

        [ConfigurationProperty(ConfigurationStrings.AllowedAudienceUris)]
        public AllowedAudienceUriElementCollection AllowedAudienceUris
        {
            get { return (AllowedAudienceUriElementCollection)base[ConfigurationStrings.AllowedAudienceUris]; }
        }

#if DESKTOP
        [ConfigurationProperty(ConfigurationStrings.AudienceUriMode, DefaultValue = IssuedTokenServiceCredential.DefaultAudienceUriMode)]
        [ServiceModelEnumValidator(typeof(AudienceUriModeValidationHelper))]
        public AudienceUriMode AudienceUriMode
        {
            get { return (AudienceUriMode)base[ConfigurationStrings.AudienceUriMode]; }
            set { base[ConfigurationStrings.AudienceUriMode] = value; }
        }
#endif

        [ConfigurationProperty(ConfigurationStrings.CustomCertificateValidatorType, DefaultValue = "")]
        [StringValidator(MinLength = 0)]
        public string CustomCertificateValidatorType
        {
            get { return (string)base[ConfigurationStrings.CustomCertificateValidatorType]; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    value = String.Empty;
                }
                base[ConfigurationStrings.CustomCertificateValidatorType] = value;
            }
        }

        [ConfigurationProperty(ConfigurationStrings.CertificateValidationMode, DefaultValue = DefaultCertificateValidationMode)]
        [ServiceModelEnumValidator(typeof(X509CertificateValidationModeHelper))]
        public X509CertificateValidationMode CertificateValidationMode
        {
            get { return (X509CertificateValidationMode)base[ConfigurationStrings.CertificateValidationMode]; }
            set { base[ConfigurationStrings.CertificateValidationMode] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.RevocationMode, DefaultValue = DefaultRevocationMode)]
        [StandardRuntimeEnumValidator(typeof(X509RevocationMode))]
        public X509RevocationMode RevocationMode
        {
            get { return (X509RevocationMode)base[ConfigurationStrings.RevocationMode]; }
            set { base[ConfigurationStrings.RevocationMode] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.TrustedStoreLocation, DefaultValue = DefaultTrustedStoreLocation)]
        [StandardRuntimeEnumValidator(typeof(StoreLocation))]
        public StoreLocation TrustedStoreLocation
        {
            get { return (StoreLocation)base[ConfigurationStrings.TrustedStoreLocation]; }
            set { base[ConfigurationStrings.TrustedStoreLocation] = value; }
        }


        [ConfigurationProperty(ConfigurationStrings.SamlSerializerType, DefaultValue = "")]
        [StringValidator(MinLength = 0)]
        public string SamlSerializerType
        {
            get { return (string)base[ConfigurationStrings.SamlSerializerType]; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    value = String.Empty;
                }
                base[ConfigurationStrings.SamlSerializerType] = value;
            }
        }

        [ConfigurationProperty(ConfigurationStrings.KnownCertificates)]
        public X509CertificateTrustedIssuerElementCollection KnownCertificates
        {
            get { return (X509CertificateTrustedIssuerElementCollection)base[ConfigurationStrings.KnownCertificates]; }
        }

        [ConfigurationProperty(ConfigurationStrings.AllowUntrustedRsaIssuers, DefaultValue = DefaultAllowUntrustedRsaIssuers)]
        public bool AllowUntrustedRsaIssuers
        {
            get { return (bool)base[ConfigurationStrings.AllowUntrustedRsaIssuers]; }
            set { base[ConfigurationStrings.AllowUntrustedRsaIssuers] = value; }
        }



        public void Copy(IssuedTokenServiceElement from)
        {
            if (this.IsReadOnly())
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ConfigurationErrorsException(SR.GetString(SR.ConfigReadOnly)));
            }
            if (null == from)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("from");
            }
            this.SamlSerializerType = from.SamlSerializerType;
            PropertyInformationCollection propertyInfo = from.ElementInformation.Properties;
            if (propertyInfo[ConfigurationStrings.KnownCertificates].ValueOrigin != PropertyValueOrigin.Default)
            {
                this.KnownCertificates.Clear();
                foreach (X509CertificateTrustedIssuerElement src in from.KnownCertificates)
                {
                    X509CertificateTrustedIssuerElement copy = new X509CertificateTrustedIssuerElement();
                    copy.Copy(src);
                    this.KnownCertificates.Add(copy);
                }
            }

            if (propertyInfo[ConfigurationStrings.AllowedAudienceUris].ValueOrigin != PropertyValueOrigin.Default)
            {
                this.AllowedAudienceUris.Clear();
                foreach (AllowedAudienceUriElement src in from.AllowedAudienceUris)
                {
                    AllowedAudienceUriElement copy = new AllowedAudienceUriElement();
                    copy.AllowedAudienceUri = src.AllowedAudienceUri;
                    this.AllowedAudienceUris.Add(copy);
                }
            }

            this.AllowUntrustedRsaIssuers = from.AllowUntrustedRsaIssuers;
            this.CertificateValidationMode = from.CertificateValidationMode;
#if DESKTOP
            this.AudienceUriMode = from.AudienceUriMode;
#endif
            this.CustomCertificateValidatorType = from.CustomCertificateValidatorType;
            this.RevocationMode = from.RevocationMode;
            this.TrustedStoreLocation = from.TrustedStoreLocation;
        }

#if DESKTOP
        internal void ApplyConfiguration(IssuedTokenServiceCredential issuedToken)
        {
            if (issuedToken == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("issuedToken");
            }
            issuedToken.CertificateValidationMode = this.CertificateValidationMode;
            issuedToken.RevocationMode = this.RevocationMode;
            issuedToken.TrustedStoreLocation = this.TrustedStoreLocation;
            issuedToken.AudienceUriMode = this.AudienceUriMode;
            if (!string.IsNullOrEmpty(this.CustomCertificateValidatorType))
            {
                Type type = System.Type.GetType(this.CustomCertificateValidatorType, true);
                if (!typeof(X509CertificateValidator).IsAssignableFrom(type))
                {
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ConfigurationErrorsException(
                        SR.GetString(SR.ConfigInvalidCertificateValidatorType, this.CustomCertificateValidatorType, typeof(X509CertificateValidator).ToString())));
                }
                issuedToken.CustomCertificateValidator = (X509CertificateValidator)Activator.CreateInstance(type);
            }
            if (!string.IsNullOrEmpty(this.SamlSerializerType))
            {
                Type type = System.Type.GetType(this.SamlSerializerType, true);
                if (!typeof(SamlSerializer).IsAssignableFrom(type))
                {
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ConfigurationErrorsException(
                        SR.GetString(SR.ConfigInvalidSamlSerializerType, this.SamlSerializerType, typeof(SamlSerializer).ToString())));
                }
                issuedToken.SamlSerializer = (SamlSerializer)Activator.CreateInstance(type);
            }
            PropertyInformationCollection propertyInfo = this.ElementInformation.Properties;
            if (propertyInfo[ConfigurationStrings.KnownCertificates].ValueOrigin != PropertyValueOrigin.Default)
            {
                foreach (X509CertificateTrustedIssuerElement src in this.KnownCertificates)
                {
                    issuedToken.KnownCertificates.Add(SecurityUtils.GetCertificateFromStore(src.StoreName, src.StoreLocation, src.X509FindType, src.FindValue, null));
                }
            }

            if (propertyInfo[ConfigurationStrings.AllowedAudienceUris].ValueOrigin != PropertyValueOrigin.Default)
            {
                foreach (AllowedAudienceUriElement src in this.AllowedAudienceUris)
                {
                    issuedToken.AllowedAudienceUris.Add(src.AllowedAudienceUri);
                }
            }

            issuedToken.AllowUntrustedRsaIssuers = this.AllowUntrustedRsaIssuers;
        }
#endif
    }
}



