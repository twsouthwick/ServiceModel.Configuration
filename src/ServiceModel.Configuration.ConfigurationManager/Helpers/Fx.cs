using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    internal static class FxTrace
    {
        internal static class Exception
        {
            internal static System.Exception ArgumentNull(string name)
            {
                return new ArgumentNullException(name);
            }
        }
    }

    internal static class Fx
    {
        internal static class Tag
        {
            internal sealed class SecurityNoteAttribute : Attribute
            {
                public string Critical { get; set; }

                public string Safe { get; set; }
            }
        }
    }
}
