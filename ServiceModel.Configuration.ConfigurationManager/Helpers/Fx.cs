using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
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
