//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------
namespace System.ServiceModel.Channels
{
    using System.ComponentModel;

    static class SecurityHeaderLayoutHelper
    {
        public static bool IsDefined(SecurityHeaderLayout value)
        {
            return (value == SecurityHeaderLayout.Lax
#if DESKTOP
            || value == SecurityHeaderLayout.LaxTimestampFirst
            || value == SecurityHeaderLayout.LaxTimestampLast
#endif
            || value == SecurityHeaderLayout.Strict);
        }

        public static void Validate(SecurityHeaderLayout value)
        {
            if (!IsDefined(value))
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidEnumArgumentException("value", (int)value,
                    typeof(SecurityHeaderLayout)));
            }
        }
    }
}
