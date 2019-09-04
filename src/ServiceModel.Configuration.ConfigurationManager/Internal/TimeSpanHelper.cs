//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------
namespace System.ServiceModel
{
    using System;
    using System.Runtime;
    using System.Globalization;
    using System.Diagnostics;

    static class TimeSpanHelper
    {
        static public TimeSpan FromMinutes(int minutes, string text)
        {
            TimeSpan value = TimeSpan.FromTicks(TimeSpan.TicksPerMinute * minutes);
            Debug.Assert(value == TimeSpan.Parse(text, CultureInfo.InvariantCulture), "");
            return value;
        }
        static public TimeSpan FromSeconds(int seconds, string text)
        {
            TimeSpan value = TimeSpan.FromTicks(TimeSpan.TicksPerSecond * seconds);
            Debug.Assert(value == TimeSpan.Parse(text, CultureInfo.InvariantCulture), "");
            return value;
        }
        static public TimeSpan FromMilliseconds(int ms, string text)
        {
            TimeSpan value = TimeSpan.FromTicks(TimeSpan.TicksPerMillisecond * ms);
            Debug.Assert(value == TimeSpan.Parse(text, CultureInfo.InvariantCulture), "");
            return value;
        }
        static public TimeSpan FromDays(int days, string text)
        {
            TimeSpan value = TimeSpan.FromTicks(TimeSpan.TicksPerDay * days);
            Debug.Assert(value == TimeSpan.Parse(text, CultureInfo.InvariantCulture), "");
            return value;
        }
    }
}
