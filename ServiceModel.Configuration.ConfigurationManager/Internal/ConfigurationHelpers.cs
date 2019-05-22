//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------
namespace System.ServiceModel.Configuration
{
    using System.Configuration;
    using System.Reflection;
    using System.Security;

    static class ConfigurationHelpers
    {
        /// Be sure to update UnsafeGetAssociatedBindingCollectionElement if you modify this method
        //internal static BindingCollectionElement GetAssociatedBindingCollectionElement(ContextInformation evaluationContext, string bindingCollectionName)
        //{
        //    BindingCollectionElement retVal = null;
        //    BindingsSection bindingsSection = (BindingsSection)ConfigurationHelpers.GetAssociatedSection(evaluationContext, ConfigurationStrings.BindingsSectionGroupPath);

        //    if (null != bindingsSection)
        //    {
        //        bindingsSection.UpdateBindingSections(evaluationContext);
        //        try
        //        {
        //            retVal = bindingsSection[bindingCollectionName];
        //        }
        //        catch (KeyNotFoundException)
        //        {
        //            throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
        //                new ConfigurationErrorsException(SR.GetString(SR.ConfigBindingExtensionNotFound,
        //                ConfigurationHelpers.GetBindingsSectionPath(bindingCollectionName))));
        //        }
        //        catch (NullReferenceException) // System.Configuration.ConfigurationElement bug
        //        {
        //            throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
        //                new ConfigurationErrorsException(SR.GetString(SR.ConfigBindingExtensionNotFound,
        //                ConfigurationHelpers.GetBindingsSectionPath(bindingCollectionName))));
        //        }
        //    }

        //    return retVal;
        //}

        //// Be sure to update GetAssociatedBindingCollectionElement if you modify this method
        //[Fx.Tag.SecurityNote(Critical = "Uses SecurityCritical method UnsafeGetAssociatedSection which elevates.")]
        //[SecurityCritical]
        //internal static BindingCollectionElement UnsafeGetAssociatedBindingCollectionElement(ContextInformation evaluationContext, string bindingCollectionName)
        //{
        //    BindingCollectionElement retVal = null;
        //    BindingsSection bindingsSection = (BindingsSection)ConfigurationHelpers.UnsafeGetAssociatedSection(evaluationContext, ConfigurationStrings.BindingsSectionGroupPath);

        //    if (null != bindingsSection)
        //    {
        //        bindingsSection.UpdateBindingSections(evaluationContext);
        //        try
        //        {
        //            retVal = bindingsSection[bindingCollectionName];
        //        }
        //        catch (KeyNotFoundException)
        //        {
        //            throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
        //                new ConfigurationErrorsException(SR.GetString(SR.ConfigBindingExtensionNotFound,
        //                ConfigurationHelpers.GetBindingsSectionPath(bindingCollectionName))));
        //        }
        //        catch (NullReferenceException) // System.Configuration.ConfigurationElement bug
        //        {
        //            throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
        //                new ConfigurationErrorsException(SR.GetString(SR.ConfigBindingExtensionNotFound,
        //                ConfigurationHelpers.GetBindingsSectionPath(bindingCollectionName))));
        //        }
        //    }

        //    return retVal;
        //}

        ///// Be sure to update UnsafeGetAssociatedEndpointCollectionElement if you modify this method
        //internal static EndpointCollectionElement GetAssociatedEndpointCollectionElement(ContextInformation evaluationContext, string endpointCollectionName)
        //{
        //    EndpointCollectionElement retVal = null;
        //    StandardEndpointsSection endpointsSection = (StandardEndpointsSection)ConfigurationHelpers.GetAssociatedSection(evaluationContext, ConfigurationStrings.StandardEndpointsSectionPath);

        //    if (null != endpointsSection)
        //    {
        //        endpointsSection.UpdateEndpointSections(evaluationContext);
        //        try
        //        {
        //            retVal = (EndpointCollectionElement)endpointsSection[endpointCollectionName];
        //        }
        //        catch (KeyNotFoundException)
        //        {
        //            throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
        //                new ConfigurationErrorsException(SR.GetString(SR.ConfigEndpointExtensionNotFound,
        //                ConfigurationHelpers.GetEndpointsSectionPath(endpointCollectionName))));
        //        }
        //        catch (NullReferenceException) // System.Configuration.ConfigurationElement bug
        //        {
        //            throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
        //                new ConfigurationErrorsException(SR.GetString(SR.ConfigEndpointExtensionNotFound,
        //                ConfigurationHelpers.GetEndpointsSectionPath(endpointCollectionName))));
        //        }
        //    }

        //    return retVal;
        //}

        //// Be sure to update GetAssociatedEndpointCollectionElement if you modify this method
        //[Fx.Tag.SecurityNote(Critical = "Uses SecurityCritical method UnsafeGetAssociatedSection which elevates.")]
        //[SecurityCritical]
        //internal static EndpointCollectionElement UnsafeGetAssociatedEndpointCollectionElement(ContextInformation evaluationContext, string endpointCollectionName)
        //{
        //    EndpointCollectionElement retVal = null;
        //    StandardEndpointsSection endpointsSection = (StandardEndpointsSection)ConfigurationHelpers.UnsafeGetAssociatedSection(evaluationContext, ConfigurationStrings.StandardEndpointsSectionPath);

        //    if (null != endpointsSection)
        //    {
        //        endpointsSection.UpdateEndpointSections(evaluationContext);
        //        try
        //        {
        //            retVal = (EndpointCollectionElement)endpointsSection[endpointCollectionName];
        //        }
        //        catch (KeyNotFoundException)
        //        {
        //            throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
        //                new ConfigurationErrorsException(SR.GetString(SR.ConfigEndpointExtensionNotFound,
        //                ConfigurationHelpers.GetEndpointsSectionPath(endpointCollectionName))));
        //        }
        //        catch (NullReferenceException) // System.Configuration.ConfigurationElement bug
        //        {
        //            throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
        //                new ConfigurationErrorsException(SR.GetString(SR.ConfigEndpointExtensionNotFound,
        //                ConfigurationHelpers.GetEndpointsSectionPath(endpointCollectionName))));
        //        }
        //    }

        //    return retVal;
        //}

        ///// Be sure to update UnsafeGetAssociatedSection if you modify this method
        //internal static object GetAssociatedSection(ContextInformation evalContext, string sectionPath)
        //{
        //    object retval = null;
        //    if (evalContext != null)
        //    {
        //        retval = evalContext.GetSection(sectionPath);
        //    }
        //    else
        //    {
        //        retval = AspNetEnvironment.Current.GetConfigurationSection(sectionPath);

        //        // Trace after call to underlying configuration system to
        //        // insure that configuration system is initialized
        //        if (DiagnosticUtility.ShouldTraceVerbose)
        //        {
        //            TraceUtility.TraceEvent(TraceEventType.Verbose, TraceCode.GetConfigurationSection,
        //                SR.GetString(SR.TraceCodeGetConfigurationSection),
        //                new StringTraceRecord("ConfigurationSection", sectionPath), null, null);
        //        }
        //    }
        //    if (retval == null)
        //    {
        //        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
        //            new ConfigurationErrorsException(SR.GetString(SR.ConfigSectionNotFound,
        //            sectionPath)));
        //    }
        //    return retval;
        //}

        // Be sure to update GetAssociatedSection if you modify this method
        internal static object UnsafeGetAssociatedSection(ContextInformation evalContext, string sectionPath)
        {
            object retval = null;
            if (evalContext != null)
            {
                retval = UnsafeGetSectionFromContext(evalContext, sectionPath);
            }
            else
            {
                //retval = AspNetEnvironment.Current.UnsafeGetConfigurationSection(sectionPath);
            }

            if (retval == null)
            {
                throw new ConfigurationErrorsException("Config section not found");
            }

            return retval;
        }

        ///// Be sure to update UnsafeGetBindingCollectionElement if you modify this method
        //internal static BindingCollectionElement GetBindingCollectionElement(string bindingCollectionName)
        //{
        //    return GetAssociatedBindingCollectionElement(null, bindingCollectionName);
        //}

        //// Be sure to update GetBindingCollectionElement if you modify this method
        //[Fx.Tag.SecurityNote(Critical = "Uses SecurityCritical method UnsafeGetAssociatedBindingCollectionElement which elevates.")]
        //[SecurityCritical]
        //internal static BindingCollectionElement UnsafeGetBindingCollectionElement(string bindingCollectionName)
        //{
        //    return UnsafeGetAssociatedBindingCollectionElement(null, bindingCollectionName);
        //}

        //internal static string GetBindingsSectionPath(string sectionName)
        //{
        //    return string.Concat(ConfigurationStrings.BindingsSectionGroupPath, "/", sectionName);
        //}

        //internal static string GetEndpointsSectionPath(string sectionName)
        //{
        //    return string.Concat(ConfigurationStrings.StandardEndpointsSectionName, "/", sectionName);
        //}

        /// Be sure to update UnsafeGetEndpointCollectionElement if you modify this method
        //internal static EndpointCollectionElement GetEndpointCollectionElement(string endpointCollectionName)
        //{
        //    return GetAssociatedEndpointCollectionElement(null, endpointCollectionName);
        //}

        //// Be sure to update GetEndpointCollectionElement if you modify this method
        //[Fx.Tag.SecurityNote(Critical = "Uses SecurityCritical method UnsafeGetAssociatedEndpointCollectionElement which elevates.")]
        //[SecurityCritical]
        //internal static EndpointCollectionElement UnsafeGetEndpointCollectionElement(string endpointCollectionName)
        //{
        //    return UnsafeGetAssociatedEndpointCollectionElement(null, endpointCollectionName);
        //}

        ///// Be sure to update UnsafeGetSection if you modify this method
        //internal static object GetSection(string sectionPath)
        //{
        //    return GetAssociatedSection(null, sectionPath);
        //}

        // Be sure to update GetSection if you modify this method
        [SecurityCritical]
        internal static object UnsafeGetSection(string sectionPath)
        {
            return UnsafeGetAssociatedSection(null, sectionPath);
        }

        //// Be sure to update UnsafeGetSection if you modify this method
        //[Fx.Tag.SecurityNote(Critical = "Uses SecurityCritical method UnsafeGetAssociatedSection which elevates.")]
        //[SecurityCritical]
        //internal static object UnsafeGetSectionNoTrace(string sectionPath)
        //{
        //    object retval = AspNetEnvironment.Current.UnsafeGetConfigurationSection(sectionPath);

        //    if (retval == null)
        //    {
        //        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(
        //            new ConfigurationErrorsException(SR.GetString(SR.ConfigSectionNotFound,
        //            sectionPath)));
        //    }

        //    return retval;
        //}

        internal static object UnsafeGetSectionFromContext(ContextInformation evalContext, string sectionPath)
        {
            return evalContext.GetSection(sectionPath);
        }

        internal static string GetSectionPath(string sectionName)
        {
            return string.Concat(ConfigurationStrings.SectionGroupName, "/", sectionName);
        }

        internal static void SetIsPresent(ConfigurationElement element)
        {
            // Work around for VSW 578830: ConfigurationElements that override DeserializeElement cannot set ElementInformation.IsPresent
            PropertyInfo elementPresent = element.GetType().GetProperty("ElementPresent", BindingFlags.Instance | BindingFlags.NonPublic);
            SetIsPresentWithAssert(elementPresent, element, true);
        }

        static void SetIsPresentWithAssert(PropertyInfo elementPresent, ConfigurationElement element, bool value)
        {
            elementPresent.SetValue(element, value, null);
        }

        internal static ContextInformation GetEvaluationContext(IConfigurationContextProviderInternal provider)
        {
            if (provider != null)
            {
                try
                {
                    return provider.GetEvaluationContext();
                }
                catch (ConfigurationErrorsException)
                {
                }
            }
            return null;
        }

        internal static ContextInformation GetOriginalEvaluationContext(IConfigurationContextProviderInternal provider)
        {
            if (provider != null)
            {
                // provider may not need this try/catch, but it doesn't hurt to do it
                try
                {
                    return provider.GetOriginalEvaluationContext();
                }
                catch (ConfigurationErrorsException)
                {
                }
            }
            return null;
        }

        //internal static void TraceExtensionTypeNotFound(ExtensionElement extensionElement)
        //{
        //    if (DiagnosticUtility.ShouldTraceWarning)
        //    {
        //        Dictionary<string, string> values = new Dictionary<string, string>(2);
        //        values.Add("ExtensionName", extensionElement.Name);
        //        values.Add("ExtensionType", extensionElement.Type);

        //        DictionaryTraceRecord traceRecord = new DictionaryTraceRecord(values);
        //        TraceUtility.TraceEvent(TraceEventType.Warning,
        //            TraceCode.ExtensionTypeNotFound,
        //            SR.GetString(SR.TraceCodeExtensionTypeNotFound),
        //            traceRecord,
        //            null,
        //            (Exception)null);
        //    }
        //}
    }

    interface IConfigurationContextProviderInternal
    {
        /// <summary>
        /// return the current ContextInformation (the protected property ConfigurationElement.EvaluationContext)
        /// this may throw ConfigurationErrorsException, caller should guard (see ConfigurationHelpers.GetEvaluationContext)
        /// </summary>
        /// <returns>result of ConfigurationElement.EvaluationContext</returns>
        ContextInformation GetEvaluationContext();

        /// <summary>
        /// return the ContextInformation that was present when the ConfigurationElement was first deserialized.
        /// if Reset was called, this will be the value of parent.GetOriginalEvaluationContext()
        /// if Reset was not called, this will be the value of this.GetEvaluationContext()
        /// </summary>
        /// <returns>result of parent's ConfigurationElement.EvaluationContext</returns>
        ContextInformation GetOriginalEvaluationContext();
    }

#pragma warning disable 618 // have not moved to the v4 security model yet
    [SecurityCritical(SecurityCriticalScope.Everything)]
#pragma warning restore 618
    struct EvaluationContextHelper
    {
        bool reset;
        ContextInformation inheritedContext;

        internal void OnReset(ConfigurationElement parent)
        {
            this.reset = true;
            this.inheritedContext = ConfigurationHelpers.GetOriginalEvaluationContext(parent as IConfigurationContextProviderInternal);
        }

        internal ContextInformation GetOriginalContext(IConfigurationContextProviderInternal owner)
        {
            if (this.reset)
            {
                // if reset, inherited context is authoritative, even if null
                return this.inheritedContext;
            }
            else
            {
                // otherwise use current
                return ConfigurationHelpers.GetEvaluationContext(owner);
            }
        }
    }
}
