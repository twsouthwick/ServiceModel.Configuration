//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Configuration;
#if DESKTOP
    using System.IdentityModel.Tokens;
#endif
    using System.Runtime;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Security;
    using System.ServiceModel.Security.Tokens;
    using System.Xml;
    using System.Linq;

    public partial class SecurityElementBase : BindingElementExtensionElement
    {
        internal const bool defaultAllowSerializedSigningTokenOnReply = false;
        internal const AuthenticationMode defaultAuthenticationMode = AuthenticationMode.SspiNegotiated;
        internal const bool defaultRequireDerivedKeys = true;

        // From SecurityBindingElement
        internal const string defaultAlgorithmSuiteString = ConfigurationStrings.Default;
        internal const bool defaultIncludeTimestamp = true;
        internal const bool defaultAllowInsecureTransport = false;
        internal const bool defaultRequireSignatureConfirmation = false;
        internal const bool defaultEnableUnsecuredResponse = false;
        internal const bool defaultProtectTokens = false;

        // From SecureConversationSecurityTokenParameters
        internal const bool defaultRequireCancellation = true;
        internal const bool defaultCanRenewSession = true;

        // if you add another variable, make sure to adjust: CopyFrom and UnMerge methods.
        SecurityBindingElement failedSecurityBindingElement = null;
        bool willX509IssuerReferenceAssertionBeWritten;
#if DESKTOP
        SecurityKeyType templateKeyType = IssuedSecurityTokenParameters.defaultKeyType;
#endif

        internal SecurityElementBase()
        {
        }

        internal bool HasImportFailed { get { return this.failedSecurityBindingElement != null; } }

        internal bool IsSecurityElementBootstrap { get; set; } // Used in serialization path to optimize Xml representation

#if DESKTOP
        [ConfigurationProperty(ConfigurationStrings.DefaultAlgorithmSuite, DefaultValue = SecurityBindingElement.defaultAlgorithmSuiteString)]
        [TypeConverter(typeof(SecurityAlgorithmSuiteConverter))]
        public SecurityAlgorithmSuite DefaultAlgorithmSuite
        {
            get { return (SecurityAlgorithmSuite)base[ConfigurationStrings.DefaultAlgorithmSuite]; }
            set { base[ConfigurationStrings.DefaultAlgorithmSuite] = value; }
        }
#endif

        [ConfigurationProperty(ConfigurationStrings.AllowSerializedSigningTokenOnReply, DefaultValue = defaultAllowSerializedSigningTokenOnReply)]
        public bool AllowSerializedSigningTokenOnReply
        {
            get { return (bool)base[ConfigurationStrings.AllowSerializedSigningTokenOnReply]; }
            set { base[ConfigurationStrings.AllowSerializedSigningTokenOnReply] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.EnableUnsecuredResponse, DefaultValue = defaultEnableUnsecuredResponse)]
        public bool EnableUnsecuredResponse
        {
            get { return (bool)base[ConfigurationStrings.EnableUnsecuredResponse]; }
            set { base[ConfigurationStrings.EnableUnsecuredResponse] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.AuthenticationMode, DefaultValue = defaultAuthenticationMode)]
        [ServiceModelEnumValidator(typeof(AuthenticationModeHelper))]
        public AuthenticationMode AuthenticationMode
        {
            get { return (AuthenticationMode)base[ConfigurationStrings.AuthenticationMode]; }
            set { base[ConfigurationStrings.AuthenticationMode] = value; }
        }

        public override Type BindingElementType
        {
            get { return typeof(SecurityBindingElement); }
        }

        [ConfigurationProperty(ConfigurationStrings.RequireDerivedKeys, DefaultValue = defaultRequireDerivedKeys)]
        public bool RequireDerivedKeys
        {
            get { return (bool)base[ConfigurationStrings.RequireDerivedKeys]; }
            set { base[ConfigurationStrings.RequireDerivedKeys] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.SecurityHeaderLayout, DefaultValue = SecurityProtocolFactory.defaultSecurityHeaderLayout)]
        [ServiceModelEnumValidator(typeof(SecurityHeaderLayoutHelper))]
        public SecurityHeaderLayout SecurityHeaderLayout
        {
            get { return (SecurityHeaderLayout)base[ConfigurationStrings.SecurityHeaderLayout]; }
            set { base[ConfigurationStrings.SecurityHeaderLayout] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.IncludeTimestamp, DefaultValue = defaultIncludeTimestamp)]
        public bool IncludeTimestamp
        {
            get { return (bool)base[ConfigurationStrings.IncludeTimestamp]; }
            set { base[ConfigurationStrings.IncludeTimestamp] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.AllowInsecureTransport, DefaultValue = defaultAllowInsecureTransport)]
        public bool AllowInsecureTransport
        {
            get { return (bool)base[ConfigurationStrings.AllowInsecureTransport]; }
            set { base[ConfigurationStrings.AllowInsecureTransport] = value; }
        }

#if DESKTOP
        [ConfigurationProperty(ConfigurationStrings.KeyEntropyMode, DefaultValue = System.ServiceModel.Security.AcceleratedTokenProvider.defaultKeyEntropyMode)]
        [ServiceModelEnumValidator(typeof(SecurityKeyEntropyModeHelper))]
        public SecurityKeyEntropyMode KeyEntropyMode
        {
            get { return (SecurityKeyEntropyMode)base[ConfigurationStrings.KeyEntropyMode]; }
            set { base[ConfigurationStrings.KeyEntropyMode] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.IssuedTokenParameters)]
        public IssuedTokenParametersElement IssuedTokenParameters
        {
            get { return (IssuedTokenParametersElement)base[ConfigurationStrings.IssuedTokenParameters]; }
        }
#endif

        [ConfigurationProperty(ConfigurationStrings.LocalClientSettings)]
        public LocalClientSecuritySettingsElement LocalClientSettings
        {
            get { return (LocalClientSecuritySettingsElement)base[ConfigurationStrings.LocalClientSettings]; }
        }

#if DESKTOP
        [ConfigurationProperty(ConfigurationStrings.LocalServiceSettings)]
        public LocalServiceSecuritySettingsElement LocalServiceSettings
        {
            get { return (LocalServiceSecuritySettingsElement)base[ConfigurationStrings.LocalServiceSettings]; }
        }

        [ConfigurationProperty(ConfigurationStrings.MessageProtectionOrder, DefaultValue = SecurityBindingElement.defaultMessageProtectionOrder)]
        [ServiceModelEnumValidator(typeof(MessageProtectionOrderHelper))]
        public MessageProtectionOrder MessageProtectionOrder
        {
            get { return (MessageProtectionOrder)base[ConfigurationStrings.MessageProtectionOrder]; }
            set { base[ConfigurationStrings.MessageProtectionOrder] = value; }
        }
#endif

        [ConfigurationProperty(ConfigurationStrings.ProtectTokens, DefaultValue = false)]
        public bool ProtectTokens
        {
            get { return (bool)base[ConfigurationStrings.ProtectTokens]; }
            set { base[ConfigurationStrings.ProtectTokens] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.MessageSecurityVersion, DefaultValue = ConfigurationStrings.Default)]
        [TypeConverter(typeof(MessageSecurityVersionConverter))]
        public MessageSecurityVersion MessageSecurityVersion
        {
            get { return (MessageSecurityVersion)base[ConfigurationStrings.MessageSecurityVersion]; }
            set { base[ConfigurationStrings.MessageSecurityVersion] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.RequireSecurityContextCancellation, DefaultValue = defaultRequireCancellation)]
        public bool RequireSecurityContextCancellation
        {
            get { return (bool)base[ConfigurationStrings.RequireSecurityContextCancellation]; }
            set { base[ConfigurationStrings.RequireSecurityContextCancellation] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.RequireSignatureConfirmation, DefaultValue = defaultRequireSignatureConfirmation)]
        public bool RequireSignatureConfirmation
        {
            get { return (bool)base[ConfigurationStrings.RequireSignatureConfirmation]; }
            set { base[ConfigurationStrings.RequireSignatureConfirmation] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.CanRenewSecurityContextToken, DefaultValue = defaultCanRenewSession)]
        public bool CanRenewSecurityContextToken
        {
            get { return (bool)base[ConfigurationStrings.CanRenewSecurityContextToken]; }
            set { base[ConfigurationStrings.CanRenewSecurityContextToken] = value; }
        }

        public override void ApplyConfiguration(BindingElement bindingElement)
        {
            base.ApplyConfiguration(bindingElement);

            SecurityBindingElement sbe = (SecurityBindingElement)bindingElement;

#if DESKTOP
            if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.DefaultAlgorithmSuite].ValueOrigin)
                sbe.DefaultAlgorithmSuite = this.DefaultAlgorithmSuite;
#endif
            if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.IncludeTimestamp].ValueOrigin)
                sbe.IncludeTimestamp = this.IncludeTimestamp;
            if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.MessageSecurityVersion].ValueOrigin)
                sbe.MessageSecurityVersion = this.MessageSecurityVersion;
#if DESKTOP
            if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.KeyEntropyMode].ValueOrigin)
                sbe.KeyEntropyMode = this.KeyEntropyMode;
#endif
            if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.SecurityHeaderLayout].ValueOrigin)
                sbe.SecurityHeaderLayout = this.SecurityHeaderLayout;
#if DESKTOP
            if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.RequireDerivedKeys].ValueOrigin)
                sbe.SetKeyDerivation(this.RequireDerivedKeys);
            if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.AllowInsecureTransport].ValueOrigin)
                sbe.AllowInsecureTransport = this.AllowInsecureTransport;
            if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.EnableUnsecuredResponse].ValueOrigin)
                sbe.EnableUnsecuredResponse = this.EnableUnsecuredResponse;
            if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.ProtectTokens].ValueOrigin)
                sbe.ProtectTokens = this.ProtectTokens;

            SymmetricSecurityBindingElement ssbe = sbe as SymmetricSecurityBindingElement;

            if (ssbe != null)
            {
                if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.MessageProtectionOrder].ValueOrigin)
                    ssbe.MessageProtectionOrder = this.MessageProtectionOrder;
                if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.RequireSignatureConfirmation].ValueOrigin)
                    ssbe.RequireSignatureConfirmation = this.RequireSignatureConfirmation;

                SecureConversationSecurityTokenParameters scParameters = ssbe.ProtectionTokenParameters as SecureConversationSecurityTokenParameters;
                if (scParameters != null)
                {
                    scParameters.CanRenewSession = this.CanRenewSecurityContextToken;
                }
            }

            AsymmetricSecurityBindingElement asbe = sbe as AsymmetricSecurityBindingElement;

            if (asbe != null)
            {
                if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.MessageProtectionOrder].ValueOrigin)
                    asbe.MessageProtectionOrder = this.MessageProtectionOrder;
                if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.RequireSignatureConfirmation].ValueOrigin)
                    asbe.RequireSignatureConfirmation = this.RequireSignatureConfirmation;
                if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.AllowSerializedSigningTokenOnReply].ValueOrigin)
                    asbe.AllowSerializedSigningTokenOnReply = this.AllowSerializedSigningTokenOnReply;
            }

            TransportSecurityBindingElement tsbe = sbe as TransportSecurityBindingElement;

            if (tsbe != null)
            {
                if (tsbe.EndpointSupportingTokenParameters.Endorsing.Count == 1)
                {
                    SecureConversationSecurityTokenParameters scParameters = tsbe.EndpointSupportingTokenParameters.Endorsing[0] as SecureConversationSecurityTokenParameters;
                    if (scParameters != null)
                    {
                        scParameters.CanRenewSession = this.CanRenewSecurityContextToken;
                    }
                }
            }
#endif

            if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.LocalClientSettings].ValueOrigin)
            {
                this.LocalClientSettings.ApplyConfiguration(sbe.LocalClientSettings);
            }

#if DESKTOP
            if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.LocalServiceSettings].ValueOrigin)
            {
                this.LocalServiceSettings.ApplyConfiguration(sbe.LocalServiceSettings);
            }
#endif
        }

        public override void CopyFrom(ServiceModelExtensionElement from)
        {
            base.CopyFrom(from);

            SecurityElementBase source = (SecurityElementBase)from;

            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.AllowSerializedSigningTokenOnReply].ValueOrigin)
                this.AllowSerializedSigningTokenOnReply = source.AllowSerializedSigningTokenOnReply;
#if DESKTOP
            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.DefaultAlgorithmSuite].ValueOrigin)
                this.DefaultAlgorithmSuite = source.DefaultAlgorithmSuite;
#endif
            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.EnableUnsecuredResponse].ValueOrigin)
                this.EnableUnsecuredResponse = source.EnableUnsecuredResponse;
            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.AllowInsecureTransport].ValueOrigin)
                this.AllowInsecureTransport = source.AllowInsecureTransport;
            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.RequireDerivedKeys].ValueOrigin)
                this.RequireDerivedKeys = source.RequireDerivedKeys;
            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.IncludeTimestamp].ValueOrigin)
                this.IncludeTimestamp = source.IncludeTimestamp;
#if DESKTOP
            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.IssuedTokenParameters].ValueOrigin)
                this.IssuedTokenParameters.Copy(source.IssuedTokenParameters);
            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.MessageProtectionOrder].ValueOrigin)
                this.MessageProtectionOrder = source.MessageProtectionOrder;
#endif
            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.ProtectTokens].ValueOrigin)
                this.ProtectTokens = source.ProtectTokens;
            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.MessageSecurityVersion].ValueOrigin)
                this.MessageSecurityVersion = source.MessageSecurityVersion;
            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.RequireSignatureConfirmation].ValueOrigin)
                this.RequireSignatureConfirmation = source.RequireSignatureConfirmation;
            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.RequireSecurityContextCancellation].ValueOrigin)
                this.RequireSecurityContextCancellation = source.RequireSecurityContextCancellation;
            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.CanRenewSecurityContextToken].ValueOrigin)
                this.CanRenewSecurityContextToken = source.CanRenewSecurityContextToken;
#if DESKTOP
            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.KeyEntropyMode].ValueOrigin)
                this.KeyEntropyMode = source.KeyEntropyMode;
#endif
            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.SecurityHeaderLayout].ValueOrigin)
                this.SecurityHeaderLayout = source.SecurityHeaderLayout;
            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.LocalClientSettings].ValueOrigin)
                this.LocalClientSettings.CopyFrom(source.LocalClientSettings);
#if DESKTOP
            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.LocalServiceSettings].ValueOrigin)
                this.LocalServiceSettings.CopyFrom(source.LocalServiceSettings);
#endif

            this.failedSecurityBindingElement = source.failedSecurityBindingElement;
            this.willX509IssuerReferenceAssertionBeWritten = source.willX509IssuerReferenceAssertionBeWritten;
        }

        protected internal override BindingElement CreateBindingElement()
        {
            return this.CreateBindingElement(false);
        }

        protected internal virtual BindingElement CreateBindingElement(bool createTemplateOnly)
        {
#if DESKTOP
            SecurityBindingElement result;
            switch (this.AuthenticationMode)
            {
                case AuthenticationMode.AnonymousForCertificate:
                    result = SecurityBindingElement.CreateAnonymousForCertificateBindingElement();
                    break;
                case AuthenticationMode.AnonymousForSslNegotiated:
                    result = SecurityBindingElement.CreateSslNegotiationBindingElement(false, this.RequireSecurityContextCancellation);
                    break;
                case AuthenticationMode.CertificateOverTransport:
                    result = SecurityBindingElement.CreateCertificateOverTransportBindingElement(this.MessageSecurityVersion);
                    break;
                case AuthenticationMode.IssuedToken:
                    result = SecurityBindingElement.CreateIssuedTokenBindingElement(this.IssuedTokenParameters.Create(createTemplateOnly, this.templateKeyType));
                    break;
                case AuthenticationMode.IssuedTokenForCertificate:
                    result = SecurityBindingElement.CreateIssuedTokenForCertificateBindingElement(this.IssuedTokenParameters.Create(createTemplateOnly, this.templateKeyType));
                    break;
                case AuthenticationMode.IssuedTokenForSslNegotiated:
                    result = SecurityBindingElement.CreateIssuedTokenForSslBindingElement(this.IssuedTokenParameters.Create(createTemplateOnly, this.templateKeyType), this.RequireSecurityContextCancellation);
                    break;
                case AuthenticationMode.IssuedTokenOverTransport:
                    result = SecurityBindingElement.CreateIssuedTokenOverTransportBindingElement(this.IssuedTokenParameters.Create(createTemplateOnly, this.templateKeyType));
                    break;
                case AuthenticationMode.Kerberos:
                    result = SecurityBindingElement.CreateKerberosBindingElement();
                    break;
                case AuthenticationMode.KerberosOverTransport:
                    result = SecurityBindingElement.CreateKerberosOverTransportBindingElement();
                    break;
                case AuthenticationMode.MutualCertificateDuplex:
                    result = SecurityBindingElement.CreateMutualCertificateDuplexBindingElement(this.MessageSecurityVersion);
                    break;
                case AuthenticationMode.MutualCertificate:
                    result = SecurityBindingElement.CreateMutualCertificateBindingElement(this.MessageSecurityVersion);
                    break;
                case AuthenticationMode.MutualSslNegotiated:
                    result = SecurityBindingElement.CreateSslNegotiationBindingElement(true, this.RequireSecurityContextCancellation);
                    break;
                case AuthenticationMode.SspiNegotiated:
                    result = SecurityBindingElement.CreateSspiNegotiationBindingElement(this.RequireSecurityContextCancellation);
                    break;
                case AuthenticationMode.UserNameForCertificate:
                    result = SecurityBindingElement.CreateUserNameForCertificateBindingElement();
                    break;
                case AuthenticationMode.UserNameForSslNegotiated:
                    result = SecurityBindingElement.CreateUserNameForSslBindingElement(this.RequireSecurityContextCancellation);
                    break;
                case AuthenticationMode.UserNameOverTransport:
                    result = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
                    break;
                case AuthenticationMode.SspiNegotiatedOverTransport:
                    result = SecurityBindingElement.CreateSspiNegotiationOverTransportBindingElement(this.RequireSecurityContextCancellation);
                    break;
                default:
                    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidEnumArgumentException("AuthenticationMode", (int)this.AuthenticationMode, typeof(AuthenticationMode)));
            }

            this.ApplyConfiguration(result);

            return result;
#else
            throw new PlatformNotSupportedException();
#endif
        }

        protected void AddBindingTemplate(Dictionary<AuthenticationMode, SecurityBindingElement> bindingTemplates, AuthenticationMode mode)
        {
            this.AuthenticationMode = mode;
            try
            {
                bindingTemplates[mode] = (SecurityBindingElement)this.CreateBindingElement(true);
            }
            catch (Exception e)
            {
                if (Fx.IsFatal(e))
                {
                    throw;
                }
            }
        }

        static bool AreTokenParametersMatching(SecurityTokenParameters p1, SecurityTokenParameters p2, bool skipRequireDerivedKeysComparison, bool exactMessageSecurityVersion)
        {
            if (p1 == null || p2 == null)
                return false;

            if (p1.GetType() != p2.GetType())
                return false;

#if DESKTOP
            if (p1.InclusionMode != p2.InclusionMode)
                return false;

            if (skipRequireDerivedKeysComparison == false && p1.RequireDerivedKeys != p2.RequireDerivedKeys)
                return false;

            if (p1.ReferenceStyle != p2.ReferenceStyle)
                return false;

            // mutual ssl and anonymous ssl differ in the client cert requirement
            if (p1 is SslSecurityTokenParameters)
            {
                if (((SslSecurityTokenParameters)p1).RequireClientCertificate != ((SslSecurityTokenParameters)p2).RequireClientCertificate)
                    return false;
            }
            else if (p1 is SecureConversationSecurityTokenParameters)
            {
                SecureConversationSecurityTokenParameters sc1 = (SecureConversationSecurityTokenParameters)p1;
                SecureConversationSecurityTokenParameters sc2 = (SecureConversationSecurityTokenParameters)p2;

                if (sc1.RequireCancellation != sc2.RequireCancellation)
                    return false;

                if (sc1.CanRenewSession != sc2.CanRenewSession)
                    return false;


                if (!AreBindingsMatching(sc1.BootstrapSecurityBindingElement, sc2.BootstrapSecurityBindingElement, exactMessageSecurityVersion))
                    return false;
            }
            else if (p1 is IssuedSecurityTokenParameters)
            {
                if (((IssuedSecurityTokenParameters)p1).KeyType != ((IssuedSecurityTokenParameters)p2).KeyType)
                    return false;
            }
#endif

            return true;
        }

        static bool AreTokenParameterCollectionsMatching(Collection<SecurityTokenParameters> c1, Collection<SecurityTokenParameters> c2, bool exactMessageSecurityVersion)
        {
            if (c1.Count != c2.Count)
                return false;

            for (int i = 0; i < c1.Count; i++)
                if (!AreTokenParametersMatching(c1[i], c2[i], true, exactMessageSecurityVersion))
                    return false;

            return true;
        }

        internal static bool AreBindingsMatching(SecurityBindingElement b1, SecurityBindingElement b2)
        {
            return AreBindingsMatching(b1, b2, true);
        }

        internal static bool AreBindingsMatching(SecurityBindingElement b1, SecurityBindingElement b2, bool exactMessageSecurityVersion)
        {
            if (b1 == null || b2 == null)
                return b1 == b2;

            if (b1.GetType() != b2.GetType())
                return false;

            if (b1.MessageSecurityVersion != b2.MessageSecurityVersion)
            {
                // exactMessageSecurityVersion meant that BSP mismatch could be ignored
                if (exactMessageSecurityVersion)
                    return false;

                if (b1.MessageSecurityVersion.SecurityVersion != b2.MessageSecurityVersion.SecurityVersion
                 || b1.MessageSecurityVersion.TrustVersion != b2.MessageSecurityVersion.TrustVersion
                 || b1.MessageSecurityVersion.SecureConversationVersion != b2.MessageSecurityVersion.SecureConversationVersion
                 || b1.MessageSecurityVersion.SecurityPolicyVersion != b2.MessageSecurityVersion.SecurityPolicyVersion)
                {
                    return false;
                }
            }

            if (b1.SecurityHeaderLayout != b2.SecurityHeaderLayout)
                return false;

#if DESKTOP
            if (b1.DefaultAlgorithmSuite != b2.DefaultAlgorithmSuite)
                return false;
#endif

            if (b1.IncludeTimestamp != b2.IncludeTimestamp)
                return false;

            if (b1.SecurityHeaderLayout != b2.SecurityHeaderLayout)
                return false;

#if DESKTOP
            if (b1.KeyEntropyMode != b2.KeyEntropyMode)
                return false;
#endif

            if (!AreTokenParameterCollectionsMatching(b1.EndpointSupportingTokenParameters.Endorsing, b2.EndpointSupportingTokenParameters.Endorsing, exactMessageSecurityVersion))
                return false;

            if (!AreTokenParameterCollectionsMatching(b1.EndpointSupportingTokenParameters.SignedEncrypted, b2.EndpointSupportingTokenParameters.SignedEncrypted, exactMessageSecurityVersion))
                return false;

#if DESKTOP
            if (!AreTokenParameterCollectionsMatching(b1.EndpointSupportingTokenParameters.Signed, b2.EndpointSupportingTokenParameters.Signed, exactMessageSecurityVersion))
                return false;

            if (!AreTokenParameterCollectionsMatching(b1.EndpointSupportingTokenParameters.SignedEndorsing, b2.EndpointSupportingTokenParameters.SignedEndorsing, exactMessageSecurityVersion))
                return false;

            if (b1.OperationSupportingTokenParameters.Count != b2.OperationSupportingTokenParameters.Count)
                return false;

            foreach (KeyValuePair<string, SupportingTokenParameters> operation1 in b1.OperationSupportingTokenParameters)
            {
                if (!b2.OperationSupportingTokenParameters.ContainsKey(operation1.Key))
                    return false;

                SupportingTokenParameters stp2 = b2.OperationSupportingTokenParameters[operation1.Key];

                if (!AreTokenParameterCollectionsMatching(operation1.Value.Endorsing, stp2.Endorsing, exactMessageSecurityVersion))
                    return false;

                if (!AreTokenParameterCollectionsMatching(operation1.Value.SignedEncrypted, stp2.SignedEncrypted, exactMessageSecurityVersion))
                    return false;

                if (!AreTokenParameterCollectionsMatching(operation1.Value.Signed, stp2.Signed, exactMessageSecurityVersion))
                    return false;

                if (!AreTokenParameterCollectionsMatching(operation1.Value.SignedEndorsing, stp2.SignedEndorsing, exactMessageSecurityVersion))
                    return false;
            }

            SymmetricSecurityBindingElement ssbe1 = b1 as SymmetricSecurityBindingElement;
            if (ssbe1 != null)
            {
                SymmetricSecurityBindingElement ssbe2 = (SymmetricSecurityBindingElement)b2;

                if (ssbe1.MessageProtectionOrder != ssbe2.MessageProtectionOrder)
                    return false;

                if (!AreTokenParametersMatching(ssbe1.ProtectionTokenParameters, ssbe2.ProtectionTokenParameters, false, exactMessageSecurityVersion))
                    return false;
            }

            AsymmetricSecurityBindingElement asbe1 = b1 as AsymmetricSecurityBindingElement;
            if (asbe1 != null)
            {
                AsymmetricSecurityBindingElement asbe2 = (AsymmetricSecurityBindingElement)b2;

                if (asbe1.MessageProtectionOrder != asbe2.MessageProtectionOrder)
                    return false;

                if (asbe1.RequireSignatureConfirmation != asbe2.RequireSignatureConfirmation)
                    return false;

                if (!AreTokenParametersMatching(asbe1.InitiatorTokenParameters, asbe2.InitiatorTokenParameters, true, exactMessageSecurityVersion)
                    || !AreTokenParametersMatching(asbe1.RecipientTokenParameters, asbe2.RecipientTokenParameters, true, exactMessageSecurityVersion))
                    return false;
            }
#endif

            return true;
        }

        protected virtual void AddBindingTemplates(Dictionary<AuthenticationMode, SecurityBindingElement> bindingTemplates)
        {
            AddBindingTemplate(bindingTemplates, AuthenticationMode.AnonymousForCertificate);
            AddBindingTemplate(bindingTemplates, AuthenticationMode.AnonymousForSslNegotiated);
            AddBindingTemplate(bindingTemplates, AuthenticationMode.CertificateOverTransport);
#if DESKTOP
            if (this.templateKeyType == SecurityKeyType.SymmetricKey)
            {
                AddBindingTemplate(bindingTemplates, AuthenticationMode.IssuedToken);
            }
#endif
            AddBindingTemplate(bindingTemplates, AuthenticationMode.IssuedTokenForCertificate);
            AddBindingTemplate(bindingTemplates, AuthenticationMode.IssuedTokenForSslNegotiated);
            AddBindingTemplate(bindingTemplates, AuthenticationMode.IssuedTokenOverTransport);
            AddBindingTemplate(bindingTemplates, AuthenticationMode.Kerberos);
            AddBindingTemplate(bindingTemplates, AuthenticationMode.KerberosOverTransport);
            AddBindingTemplate(bindingTemplates, AuthenticationMode.MutualCertificate);
            AddBindingTemplate(bindingTemplates, AuthenticationMode.MutualCertificateDuplex);
            AddBindingTemplate(bindingTemplates, AuthenticationMode.MutualSslNegotiated);
            AddBindingTemplate(bindingTemplates, AuthenticationMode.SspiNegotiated);
            AddBindingTemplate(bindingTemplates, AuthenticationMode.UserNameForCertificate);
            AddBindingTemplate(bindingTemplates, AuthenticationMode.UserNameForSslNegotiated);
            AddBindingTemplate(bindingTemplates, AuthenticationMode.UserNameOverTransport);
            AddBindingTemplate(bindingTemplates, AuthenticationMode.SspiNegotiatedOverTransport);
        }

        bool TryInitializeAuthenticationMode(SecurityBindingElement sbe)
        {
            bool result;

#if DESKTOP
            if (sbe.OperationSupportingTokenParameters.Count > 0)
                result = false;
            else
#endif
            {
                SetIssuedTokenKeyType(sbe);

                Dictionary<AuthenticationMode, SecurityBindingElement> bindingTemplates = new Dictionary<AuthenticationMode, SecurityBindingElement>();
                this.AddBindingTemplates(bindingTemplates);

                result = false;
                foreach (AuthenticationMode mode in bindingTemplates.Keys)
                {
                    SecurityBindingElement candidate = bindingTemplates[mode];
                    if (AreBindingsMatching(sbe, candidate))
                    {
                        this.AuthenticationMode = mode;
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        void SetIssuedTokenKeyType(SecurityBindingElement sbe)
        {
#if DESKTOP
            // Set the keyType for building the template for IssuedToken binding.
            // The reason is the different supporting token is defined depending on keyType.
            if (sbe.EndpointSupportingTokenParameters.Endorsing.Count > 0 &&
                sbe.EndpointSupportingTokenParameters.Endorsing[0] is IssuedSecurityTokenParameters)
            {
                this.templateKeyType = ((IssuedSecurityTokenParameters)sbe.EndpointSupportingTokenParameters.Endorsing[0]).KeyType;
            }
            else if (sbe.EndpointSupportingTokenParameters.Signed.Count > 0 &&
                sbe.EndpointSupportingTokenParameters.Signed[0] is IssuedSecurityTokenParameters)
            {
                this.templateKeyType = ((IssuedSecurityTokenParameters)sbe.EndpointSupportingTokenParameters.Signed[0]).KeyType;
            }
            else if (sbe.EndpointSupportingTokenParameters.SignedEncrypted.Count > 0 &&
                sbe.EndpointSupportingTokenParameters.SignedEncrypted[0] is IssuedSecurityTokenParameters)
            {
                this.templateKeyType = ((IssuedSecurityTokenParameters)sbe.EndpointSupportingTokenParameters.SignedEncrypted[0]).KeyType;
            }
            else
            {
                this.templateKeyType = IssuedSecurityTokenParameters.defaultKeyType;
            }
#endif
        }

        protected virtual void InitializeNestedTokenParameterSettings(SecurityTokenParameters sp, bool initializeNestedBindings)
        {
#if DESKTOP
            if (sp is SspiSecurityTokenParameters)
                SetPropertyValueIfNotDefaultValue(ConfigurationStrings.RequireSecurityContextCancellation, ((SspiSecurityTokenParameters)sp).RequireCancellation);
            else if (sp is SslSecurityTokenParameters)
                SetPropertyValueIfNotDefaultValue(ConfigurationStrings.RequireSecurityContextCancellation, ((SslSecurityTokenParameters)sp).RequireCancellation);
            else if (sp is IssuedSecurityTokenParameters)
                this.IssuedTokenParameters.InitializeFrom((IssuedSecurityTokenParameters)sp, initializeNestedBindings);
#endif
        }

        internal void InitializeFrom(BindingElement bindingElement, bool initializeNestedBindings)
        {
            if (bindingElement == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("bindingElement");
            }
            SecurityBindingElement sbe = (SecurityBindingElement)bindingElement;

            // Can't apply default value optimization to properties like DefaultAlgorithmSuite because the defaults are computed at runtime and don't match config defaults
#if DESKTOP
            this.DefaultAlgorithmSuite = sbe.DefaultAlgorithmSuite;
#endif
            this.IncludeTimestamp = sbe.IncludeTimestamp;
#if DESKTOP
            if (sbe.MessageSecurityVersion != MessageSecurityVersion.Default)
#endif
            {
                this.MessageSecurityVersion = sbe.MessageSecurityVersion;
            }
            // Still safe to apply the optimization here because the runtime defaults are the same as config defaults in all cases
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.SecurityHeaderLayout, sbe.SecurityHeaderLayout);
#if DESKTOP
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.KeyEntropyMode, sbe.KeyEntropyMode);
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.ProtectTokens, sbe.ProtectTokens);
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.AllowInsecureTransport, sbe.AllowInsecureTransport);
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.EnableUnsecuredResponse, sbe.EnableUnsecuredResponse);

            Nullable<bool> requireDerivedKeys = new Nullable<bool>();
#endif

            if (sbe.EndpointSupportingTokenParameters.Endorsing.Count == 1)
            {
                this.InitializeNestedTokenParameterSettings(sbe.EndpointSupportingTokenParameters.Endorsing[0], initializeNestedBindings);
            }
            else if (sbe.EndpointSupportingTokenParameters.SignedEncrypted.Count == 1)
            {
                this.InitializeNestedTokenParameterSettings(sbe.EndpointSupportingTokenParameters.SignedEncrypted[0], initializeNestedBindings);
            }
#if DESKTOP
            else if (sbe.EndpointSupportingTokenParameters.Signed.Count == 1)
            {
                this.InitializeNestedTokenParameterSettings(sbe.EndpointSupportingTokenParameters.Signed[0], initializeNestedBindings);
            }
#endif

            bool initializationFailure = false;

#if DESKTOP
            foreach (SecurityTokenParameters t in sbe.EndpointSupportingTokenParameters.Endorsing)
            {
                if (t.HasAsymmetricKey == false)
                {
                    if (requireDerivedKeys.HasValue && requireDerivedKeys.Value != t.RequireDerivedKeys)
                        initializationFailure = true;
                    else
                        requireDerivedKeys = t.RequireDerivedKeys;
                }
            }

            SymmetricSecurityBindingElement ssbe = sbe as SymmetricSecurityBindingElement;
            if ( ssbe != null )
            {
                SetPropertyValueIfNotDefaultValue(ConfigurationStrings.MessageProtectionOrder, ssbe.MessageProtectionOrder);
                this.RequireSignatureConfirmation = ssbe.RequireSignatureConfirmation;
                if ( ssbe.ProtectionTokenParameters != null )
                {
                    this.InitializeNestedTokenParameterSettings( ssbe.ProtectionTokenParameters, initializeNestedBindings );
                    if ( requireDerivedKeys.HasValue && requireDerivedKeys.Value != ssbe.ProtectionTokenParameters.RequireDerivedKeys )
                        initializationFailure = true;
                    else
                        requireDerivedKeys = ssbe.ProtectionTokenParameters.RequireDerivedKeys;

                }
            }
            else
            {
                AsymmetricSecurityBindingElement asbe = sbe as AsymmetricSecurityBindingElement;
                if ( asbe != null )
                {
                    SetPropertyValueIfNotDefaultValue(ConfigurationStrings.MessageProtectionOrder, asbe.MessageProtectionOrder);
                    this.RequireSignatureConfirmation = asbe.RequireSignatureConfirmation;
                    if ( asbe.InitiatorTokenParameters != null )
                    {
                        this.InitializeNestedTokenParameterSettings( asbe.InitiatorTokenParameters, initializeNestedBindings );

                        //
                        // Copy the derived key token bool flag from the token parameters. The token parameter was set from
                        // importing WSDL during SecurityBindingElementImporter.ImportPolicy time
                        //
                        if ( requireDerivedKeys.HasValue && requireDerivedKeys.Value != asbe.InitiatorTokenParameters.RequireDerivedKeys )
                            initializationFailure = true;
                        else
                            requireDerivedKeys = asbe.InitiatorTokenParameters.RequireDerivedKeys;
                    }
                }
            }
#endif

            this.willX509IssuerReferenceAssertionBeWritten = DoesSecurityBindingElementContainClauseTypeofIssuerSerial(sbe);
#if DESKTOP
            this.RequireDerivedKeys = requireDerivedKeys.GetValueOrDefault(SecurityTokenParameters.defaultRequireDerivedKeys);
#endif
            this.LocalClientSettings.InitializeFrom(sbe.LocalClientSettings);
#if DESKTOP
            this.LocalServiceSettings.InitializeFrom(sbe.LocalServiceSettings);
#endif

            if (!initializationFailure)
                initializationFailure = !this.TryInitializeAuthenticationMode(sbe);

            if (initializationFailure)
                this.failedSecurityBindingElement = sbe;
        }

        protected internal override void InitializeFrom(BindingElement bindingElement)
        {
            this.InitializeFrom(bindingElement, true);
        }

        /// <summary>
        /// returns true if one of the xxxSupportingTokenParameters.yyy is of type IssuerSerial
        /// </summary>
        /// <param name="sbe"></param>
        /// <returns></returns>
        bool DoesSecurityBindingElementContainClauseTypeofIssuerSerial( SecurityBindingElement sbe )
        {
            if ( sbe == null )
                return false;

#if DESKTOP
            if ( sbe is SymmetricSecurityBindingElement )
            {
                X509SecurityTokenParameters tokenParamameters = ( (SymmetricSecurityBindingElement)sbe ).ProtectionTokenParameters as X509SecurityTokenParameters;
                if ( tokenParamameters != null && tokenParamameters.X509ReferenceStyle == X509KeyIdentifierClauseType.IssuerSerial )
                    return true;
            }
            else if ( sbe is AsymmetricSecurityBindingElement )
            {
                X509SecurityTokenParameters initiatorParamameters = ( (AsymmetricSecurityBindingElement)sbe ).InitiatorTokenParameters as X509SecurityTokenParameters;
                if ( initiatorParamameters != null && initiatorParamameters.X509ReferenceStyle == X509KeyIdentifierClauseType.IssuerSerial )
                    return true;

                X509SecurityTokenParameters recepientParamameters = ( (AsymmetricSecurityBindingElement)sbe ).RecipientTokenParameters as X509SecurityTokenParameters;
                if ( recepientParamameters != null && recepientParamameters.X509ReferenceStyle == X509KeyIdentifierClauseType.IssuerSerial )
                    return true;
            }
#endif

            if ( DoesX509TokenParametersContainClauseTypeofIssuerSerial( sbe.EndpointSupportingTokenParameters.Endorsing ) )
                return true;

#if DESKTOP
            if ( DoesX509TokenParametersContainClauseTypeofIssuerSerial( sbe.EndpointSupportingTokenParameters.Signed ) )
                return true;
#endif

            if ( DoesX509TokenParametersContainClauseTypeofIssuerSerial( sbe.EndpointSupportingTokenParameters.SignedEncrypted ) )
                return true;

#if DESKTOP
            if ( DoesX509TokenParametersContainClauseTypeofIssuerSerial( sbe.EndpointSupportingTokenParameters.SignedEndorsing ) )
                return true;

            if ( DoesX509TokenParametersContainClauseTypeofIssuerSerial( sbe.OptionalEndpointSupportingTokenParameters.Endorsing ) )
                return true;

            if ( DoesX509TokenParametersContainClauseTypeofIssuerSerial( sbe.OptionalEndpointSupportingTokenParameters.Signed ) )
                return true;

            if ( DoesX509TokenParametersContainClauseTypeofIssuerSerial( sbe.OptionalEndpointSupportingTokenParameters.SignedEncrypted ) )
                return true;

            if ( DoesX509TokenParametersContainClauseTypeofIssuerSerial( sbe.OptionalEndpointSupportingTokenParameters.SignedEndorsing ) )
                return true;
#endif

            return false;
        }

        bool DoesX509TokenParametersContainClauseTypeofIssuerSerial( Collection<SecurityTokenParameters> tokenParameters )
        {
#if DESKTOP
            foreach ( SecurityTokenParameters tokenParameter in tokenParameters )
            {
                X509SecurityTokenParameters x509TokenParameter = tokenParameter as X509SecurityTokenParameters;
                if ( x509TokenParameter != null )
                {
                    if ( x509TokenParameter.X509ReferenceStyle == X509KeyIdentifierClauseType.IssuerSerial )
                        return true;
                }
            }
#endif

            return false;
        }

        protected override bool SerializeToXmlElement(XmlWriter writer, String elementName)
        {
            bool result;

            if (this.failedSecurityBindingElement != null && writer != null)
            {
                writer.WriteComment(SR.GetString(SR.ConfigurationSchemaInsuffientForSecurityBindingElementInstance));
                writer.WriteComment(this.failedSecurityBindingElement.ToString());
                result = true;
            }
            else
            {
                if ( writer != null && this.willX509IssuerReferenceAssertionBeWritten )
                    writer.WriteComment( SR.GetString(SR.ConfigurationSchemaContainsX509IssuerSerialReference));

                result = base.SerializeToXmlElement(writer, elementName);
            }

            return result;
        }

        protected override bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
        {
            bool nontrivial = base.SerializeElement(writer, serializeCollectionKey);

            // A SecurityElement can copy properties from a "bootstrap" SecurityBaseElement.
            // In this case, a trivial bootstrap (no properties set) is equivalent to not having one at all so we can omit it.
            Func<PropertyInformation, bool> nontrivialProperty = property => property.ValueOrigin == PropertyValueOrigin.SetHere;
            if (this.IsSecurityElementBootstrap && !this.ElementInformation.Properties.OfType<PropertyInformation>().Any(nontrivialProperty))
            {
                nontrivial = false;
            }
            return nontrivial;
        }


        protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
        {
            if ( sourceElement is SecurityElementBase )
            {
                this.failedSecurityBindingElement = ( (SecurityElementBase)sourceElement ).failedSecurityBindingElement;
                this.willX509IssuerReferenceAssertionBeWritten = ( (SecurityElementBase)sourceElement ).willX509IssuerReferenceAssertionBeWritten;
            }

            base.Unmerge(sourceElement, parentElement, saveMode);
        }
    }
}



