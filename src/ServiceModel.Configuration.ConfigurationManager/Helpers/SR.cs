using System.Globalization;

namespace System
{
    internal partial class SR
    {
        internal static string GetString(string format) => format;

        internal static string GetString(string format, object arg) => string.Format(CultureInfo.InvariantCulture, format, arg);

        internal static string GetString(string format, object arg0, object arg1) => string.Format(CultureInfo.InvariantCulture, format, arg0, arg1);

        internal static string GetString(string format, object arg0, object arg1, object arg2) => string.Format(CultureInfo.InvariantCulture, format, arg0, arg1, arg2);
    }
}
