using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Permissions;
using System.Text;

namespace System
{
    internal static class InternalSR
    {
        public static string IncompatibleArgumentType(Type type1, Type type2)
        {
            return string.Empty;
        }
    }

    internal static class FxTrace
    {
        internal static class Exception
        {
            internal static System.Exception Argument(string name, string message)
            {
                return new ArgumentException(message, name);
            }
            internal static System.Exception ArgumentNull(string name)
            {
                return new ArgumentNullException(name);
            }
        }
    }

#if !DESKTOP
    internal class ConfigurationPermissionAttribute : Attribute
    {
        public ConfigurationPermissionAttribute(SecurityAction assert)
        {
        }

        public bool Unrestricted { get; set; }
    }
#endif

    internal static class FxCop
    {
        internal static class Category
        {
            internal const string Configuration = "Configuration";
        }
    }

    internal static class Fx
    {
        public static void Assert(bool condition, string message) => Debug.Assert(condition, message);

        public static void Assert(string message) => Assert(false, message);

        internal static class Tag
        {
            internal sealed class SecurityNoteAttribute : Attribute
            {
                public string Critical { get; set; }

                public string Safe { get; set; }

                public string Miscellaneous { get; set; }
            }
        }
    }
}
