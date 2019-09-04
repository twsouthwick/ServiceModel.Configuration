using System;
using System.Linq;
using System.ServiceModel.Channels;

namespace System
{
    internal static class Extensions
    {
        public static AddressHeader[] ToArray(this AddressHeaderCollection collection) => collection.OfType<AddressHeader>().ToArray();
    }
}
