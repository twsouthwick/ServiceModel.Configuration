using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;

namespace System
{
    internal static class Extensions
    {
        public static AddressHeader[] ToArray(this AddressHeaderCollection collection) => collection.OfType<AddressHeader>().ToArray();
    }
}
