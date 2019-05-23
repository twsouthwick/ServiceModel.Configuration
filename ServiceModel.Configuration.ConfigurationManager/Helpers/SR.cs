using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace System
{
    internal static class SR
    {
        public const string ID0003 = "";
        public const string ID0005 = "";
        public const string ID0006 = "";
        public const string ID1024 = "";
        public const string GenericCallbackException = "";
        public const string StringNullOrEmpty = "";
        public const string ConfigInvalidTypeForBindingElement = "";
        public const string ConfigBindingExtensionNotFound = "";
        public const string ConfigInvalidSection = "";
        public const string ConfigInvalidBindingName = "";
        public const string ConfigExtensionCollectionNotFound = "";
        public const string ConfigExtensionTypeNotRegisteredInCollection = "";
        public const string ConfigSectionNotFound = "";
        public const string ConfigInvalidEncodingValue = "";
        public const string ConfigDuplicateExtensionName = "";
        public const string ConfigElementKeyNull = "";
        public const string ConfigInvalidStartValue = "";
        public const string ConfigKeyNotFoundInElementCollection = "";
        public const string ConfigKeysDoNotMatch = "";
        public const string ConfigDuplicateKeyAtSameScope = "";
        public const string ConfigNoExtensionCollectionAssociatedWithType = "";
        public const string ConfigInvalidTypeForBinding = "";
        public const string ConfigReadOnly = "";
        public const string ConfigDuplicateExtensionType = "";
        public const string ConfigElementKeysNull = "";

        internal static string GetString(string name) => "ERROR!";

        internal static string GetString(string name, object arg) => GetString(name);

        internal static string GetString(string name, object arg0, object arg1) => GetString(name);

        internal static string GetString(string name, object arg0, object arg1, object arg2) => GetString(name);
    }
}
