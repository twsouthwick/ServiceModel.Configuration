//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace System.ServiceModel.Configuration
{
    using System.ComponentModel;
    using System.Configuration;
    using System.Runtime;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Security;

    public sealed partial class LocalClientSecuritySettingsElement : ServiceModelConfigurationElement
    {
        // From SpnegoTokenProvider
        internal const string defaultClientMaxTokenCachingTimeString = "10675199.02:48:05.4775807";
        internal const int defaultServiceTokenValidityThresholdPercentage = 60;
        internal const bool defaultClientCacheTokens = true;

        // From SecuritySessionClientSettings
        internal const bool defaultTolerateTransportFailures = true;

        // From SecuritySessionClientSettings
        internal const string defaultKeyRenewalIntervalString = "10:00:00";
        internal const string defaultKeyRolloverIntervalString = "00:05:00";

        public LocalClientSecuritySettingsElement()
        {
        }

        [ConfigurationProperty(ConfigurationStrings.CacheCookies, DefaultValue = defaultClientCacheTokens)]
        public bool CacheCookies
        {
            get { return (bool)base[ConfigurationStrings.CacheCookies]; }
            set { base[ConfigurationStrings.CacheCookies] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.DetectReplays, DefaultValue = SecurityProtocolFactory.defaultDetectReplays)]
        public bool DetectReplays
        {
            get { return (bool)base[ConfigurationStrings.DetectReplays]; }
            set { base[ConfigurationStrings.DetectReplays] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.ReplayCacheSize, DefaultValue = SecurityProtocolFactory.defaultMaxCachedNonces)]
        [IntegerValidator(MinValue = 1)]
        public int ReplayCacheSize
        {
            get { return (int)base[ConfigurationStrings.ReplayCacheSize]; }
            set { base[ConfigurationStrings.ReplayCacheSize] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.MaxClockSkew, DefaultValue = SecurityProtocolFactory.defaultMaxClockSkewString)]
        [TypeConverter(typeof(TimeSpanOrInfiniteConverter))]
        [ServiceModelTimeSpanValidator(MinValueString = ConfigurationStrings.TimeSpanZero)]
        public TimeSpan MaxClockSkew
        {
            get { return (TimeSpan)base[ConfigurationStrings.MaxClockSkew]; }
            set { base[ConfigurationStrings.MaxClockSkew] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.MaxCookieCachingTime, DefaultValue = defaultClientMaxTokenCachingTimeString)]
        [TypeConverter(typeof(TimeSpanOrInfiniteConverter))]
        [ServiceModelTimeSpanValidator(MinValueString = ConfigurationStrings.TimeSpanZero)]
        public TimeSpan MaxCookieCachingTime
        {
            get { return (TimeSpan)base[ConfigurationStrings.MaxCookieCachingTime]; }
            set { base[ConfigurationStrings.MaxCookieCachingTime] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.ReplayWindow, DefaultValue = SecurityProtocolFactory.defaultReplayWindowString)]
        [TypeConverter(typeof(TimeSpanOrInfiniteConverter))]
        [ServiceModelTimeSpanValidator(MinValueString = ConfigurationStrings.TimeSpanZero)]
        public TimeSpan ReplayWindow
        {
            get { return (TimeSpan)base[ConfigurationStrings.ReplayWindow]; }
            set { base[ConfigurationStrings.ReplayWindow] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.SessionKeyRenewalInterval, DefaultValue = defaultKeyRenewalIntervalString)]
        [TypeConverter(typeof(TimeSpanOrInfiniteConverter))]
        [ServiceModelTimeSpanValidator(MinValueString = ConfigurationStrings.TimeSpanZero)]
        public TimeSpan SessionKeyRenewalInterval
        {
            get { return (TimeSpan)base[ConfigurationStrings.SessionKeyRenewalInterval]; }
            set { base[ConfigurationStrings.SessionKeyRenewalInterval] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.SessionKeyRolloverInterval, DefaultValue = defaultKeyRolloverIntervalString)]
        [TypeConverter(typeof(TimeSpanOrInfiniteConverter))]
        [ServiceModelTimeSpanValidator(MinValueString = ConfigurationStrings.TimeSpanZero)]
        public TimeSpan SessionKeyRolloverInterval
        {
            get { return (TimeSpan)base[ConfigurationStrings.SessionKeyRolloverInterval]; }
            set { base[ConfigurationStrings.SessionKeyRolloverInterval] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.ReconnectTransportOnFailure, DefaultValue = defaultTolerateTransportFailures)]
        public bool ReconnectTransportOnFailure
        {
            get { return (bool)base[ConfigurationStrings.ReconnectTransportOnFailure]; }
            set { base[ConfigurationStrings.ReconnectTransportOnFailure] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.TimestampValidityDuration, DefaultValue = SecurityProtocolFactory.defaultTimestampValidityDurationString)]
        [TypeConverter(typeof(TimeSpanOrInfiniteConverter))]
        [ServiceModelTimeSpanValidator(MinValueString = ConfigurationStrings.TimeSpanZero)]
        public TimeSpan TimestampValidityDuration
        {
            get { return (TimeSpan)base[ConfigurationStrings.TimestampValidityDuration]; }
            set { base[ConfigurationStrings.TimestampValidityDuration] = value; }
        }

        [ConfigurationProperty(ConfigurationStrings.CookieRenewalThresholdPercentage, DefaultValue = defaultServiceTokenValidityThresholdPercentage)]
        [IntegerValidator(MinValue = 0, MaxValue = 100)]
        public int CookieRenewalThresholdPercentage
        {
            get { return (int)base[ConfigurationStrings.CookieRenewalThresholdPercentage]; }
            set { base[ConfigurationStrings.CookieRenewalThresholdPercentage] = value; }
        }

        internal void ApplyConfiguration(LocalClientSecuritySettings settings)
        {
            if (settings == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("settings");
            }
#if DESKTOP
            settings.CacheCookies = this.CacheCookies;
            if (PropertyValueOrigin.Default != this.ElementInformation.Properties[ConfigurationStrings.DetectReplays].ValueOrigin)
                settings.DetectReplays = this.DetectReplays;
            settings.MaxCookieCachingTime = this.MaxCookieCachingTime;
            settings.ReconnectTransportOnFailure = this.ReconnectTransportOnFailure;
            settings.ReplayCacheSize = this.ReplayCacheSize;
            settings.SessionKeyRenewalInterval = this.SessionKeyRenewalInterval;
            settings.SessionKeyRolloverInterval = this.SessionKeyRolloverInterval;
            settings.CookieRenewalThresholdPercentage = this.CookieRenewalThresholdPercentage;
#endif
            settings.MaxClockSkew = this.MaxClockSkew;
            settings.ReplayWindow = this.ReplayWindow;
            settings.TimestampValidityDuration = this.TimestampValidityDuration;
        }

        internal void InitializeFrom(LocalClientSecuritySettings settings)
        {
            if (settings == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("settings");
            }

#if DESKTOP
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.CacheCookies, settings.CacheCookies);
            this.DetectReplays = settings.DetectReplays; // can't use default value optimization here because ApplyConfiguration looks at ValueOrigin
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.MaxCookieCachingTime, settings.MaxCookieCachingTime);
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.ReconnectTransportOnFailure, settings.ReconnectTransportOnFailure);
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.ReplayCacheSize, settings.ReplayCacheSize);
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.SessionKeyRenewalInterval, settings.SessionKeyRenewalInterval);
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.SessionKeyRolloverInterval, settings.SessionKeyRolloverInterval);
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.CookieRenewalThresholdPercentage, settings.CookieRenewalThresholdPercentage);
#endif

            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.TimestampValidityDuration, settings.TimestampValidityDuration);
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.ReplayWindow, settings.ReplayWindow);
            SetPropertyValueIfNotDefaultValue(ConfigurationStrings.MaxClockSkew, settings.MaxClockSkew);
        }

        internal void CopyFrom(LocalClientSecuritySettingsElement source)
        {
            if (source == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("source");
            }
            this.CacheCookies = source.CacheCookies;
            if (PropertyValueOrigin.Default != source.ElementInformation.Properties[ConfigurationStrings.DetectReplays].ValueOrigin)
                this.DetectReplays = source.DetectReplays;
            this.MaxClockSkew = source.MaxClockSkew;
            this.MaxCookieCachingTime = source.MaxCookieCachingTime;
            this.ReconnectTransportOnFailure = source.ReconnectTransportOnFailure;
            this.ReplayCacheSize = source.ReplayCacheSize;
            this.ReplayWindow = source.ReplayWindow;
            this.SessionKeyRenewalInterval = source.SessionKeyRenewalInterval;
            this.SessionKeyRolloverInterval = source.SessionKeyRolloverInterval;
            this.TimestampValidityDuration = source.TimestampValidityDuration;
            this.CookieRenewalThresholdPercentage = source.CookieRenewalThresholdPercentage;
        }
    }
}



