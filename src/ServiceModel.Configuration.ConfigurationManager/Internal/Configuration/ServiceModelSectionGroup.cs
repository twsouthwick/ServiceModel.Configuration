//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System.Configuration;
    using System.ServiceModel.Channels;
    using System.ServiceModel;

    public sealed class ServiceModelSectionGroup : ConfigurationSectionGroup
    {
        public ServiceModelSectionGroup() { }

#if DESKTOP
        public BehaviorsSection Behaviors
        {
            get { return (BehaviorsSection)this.Sections[ConfigurationStrings.BehaviorsSectionName]; }
        }
#endif

        public BindingsSection Bindings
        {
            get { return (BindingsSection)this.Sections[ConfigurationStrings.BindingsSectionGroupName]; }
        }

        public ClientSection Client
        {
            get { return (ClientSection)this.Sections[ConfigurationStrings.ClientSectionName]; }
        }

#if DESKTOP
        public ComContractsSection ComContracts
        {
            get { return (ComContractsSection)this.Sections[ConfigurationStrings.ComContractsSectionName]; }
        }

        public CommonBehaviorsSection CommonBehaviors
        {
            get { return (CommonBehaviorsSection)this.Sections[ConfigurationStrings.CommonBehaviorsSectionName]; }
        }

        public DiagnosticSection Diagnostic
        {
            get { return (DiagnosticSection)this.Sections[ConfigurationStrings.DiagnosticSectionName]; }
        }

        public ServiceHostingEnvironmentSection ServiceHostingEnvironment
        {
            get { return (ServiceHostingEnvironmentSection)this.Sections[ConfigurationStrings.ServiceHostingEnvironmentSectionName]; }
        }
#endif

        public ExtensionsSection Extensions
        {
            get { return (ExtensionsSection)this.Sections[ConfigurationStrings.Extensions]; }
        }

#if DESKTOP
        public ProtocolMappingSection ProtocolMapping
        {
            get { return (ProtocolMappingSection)this.Sections[ConfigurationStrings.ProtocolMappingSectionName]; }
        }
#endif

        public ServicesSection Services
        {
            get { return (ServicesSection)this.Sections[ConfigurationStrings.ServicesSectionName]; }
        }

#if DESKTOP
        public StandardEndpointsSection StandardEndpoints
        {
            get { return (StandardEndpointsSection)this.Sections[ConfigurationStrings.StandardEndpointsSectionName]; }
        }
#endif

        public static ServiceModelSectionGroup GetSectionGroup(Configuration config)
        {
            if (config == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("config");
            }

            return (ServiceModelSectionGroup)config.SectionGroups[ConfigurationStrings.SectionGroupName];
        }
    }
}
