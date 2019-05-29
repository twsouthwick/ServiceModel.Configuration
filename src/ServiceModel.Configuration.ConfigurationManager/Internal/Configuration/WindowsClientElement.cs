//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System;
    using System.ServiceModel;
    using System.Configuration;
    using System.ServiceModel.Security;
    using System.ServiceModel.Channels;
    using System.Xml;
    using System.Security.Principal;
    using System.Security.Cryptography.X509Certificates;

    public sealed partial class WindowsClientElement : ConfigurationElement
    {
        // From SspiSecurityTokenProvider
        internal const bool DefaultAllowNtlm = true;
        internal const bool DefaultExtractWindowsGroupClaims = true;
        internal const bool DefaultAllowUnauthenticatedCallers = false;

        // From WindowsClientCredential
        internal const TokenImpersonationLevel DefaultImpersonationLevel = TokenImpersonationLevel.Identification;

        public WindowsClientElement()
        {
        }

        [ConfigurationProperty(ConfigurationStrings.AllowNtlm, DefaultValue = DefaultAllowNtlm)]
        public bool AllowNtlm
        {
            get { return (bool)base[ConfigurationStrings.AllowNtlm]; }
            set { base[ConfigurationStrings.AllowNtlm] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.AllowedImpersonationLevel, DefaultValue = DefaultImpersonationLevel)]
        [ServiceModelEnumValidator(typeof(TokenImpersonationLevelHelper))]
        public TokenImpersonationLevel AllowedImpersonationLevel
        {
            get { return (TokenImpersonationLevel)base[ConfigurationStrings.AllowedImpersonationLevel]; }
            set { base[ConfigurationStrings.AllowedImpersonationLevel] = value; }
        }

        public void Copy(WindowsClientElement from)
        {
            if (this.IsReadOnly())
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ConfigurationErrorsException(SR.GetString(SR.ConfigReadOnly)));
            }
            if (null == from)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("from");
            }

            this.AllowNtlm = from.AllowNtlm;
            this.AllowedImpersonationLevel = from.AllowedImpersonationLevel;
        }

        internal void ApplyConfiguration(WindowsClientCredential windows)
        {
            if (windows == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("windows");
            }

#if DESKTOP
            windows.AllowNtlm = this.AllowNtlm;
#endif

            windows.AllowedImpersonationLevel = this.AllowedImpersonationLevel;
        }
    }
}



