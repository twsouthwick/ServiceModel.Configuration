//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace System.ServiceModel.Diagnostics
{
    using System;
    using System.Diagnostics;
    using System.Xml;

    using TraceSR = SR;

    class ExceptionUtility
    {
        const string ExceptionStackAsStringKey = "System.ServiceModel.Diagnostics.ExceptionUtility.ExceptionStackAsString";

        // This field should be only used for debug build.
        internal static ExceptionUtility mainInstance;

        internal Exception ThrowHelper(Exception exception, TraceEventType eventType, object _) => exception;

        internal Exception ThrowHelper(Exception exception, TraceEventType eventType)
        {
            return this.ThrowHelper(exception, eventType, null);
        }

        internal ArgumentException ThrowHelperArgument(string message)
        {
            return (ArgumentException)this.ThrowHelperError(new ArgumentException(message));
        }

        internal ArgumentException ThrowHelperArgument(string paramName, string message)
        {
            return (ArgumentException)this.ThrowHelperError(new ArgumentException(message, paramName));
        }

        internal ArgumentNullException ThrowHelperArgumentNull(string paramName)
        {
            return (ArgumentNullException)this.ThrowHelperError(new ArgumentNullException(paramName));
        }

        internal ArgumentNullException ThrowHelperArgumentNull(string paramName, string message)
        {
            return (ArgumentNullException)this.ThrowHelperError(new ArgumentNullException(paramName, message));
        }

        internal ArgumentException ThrowHelperArgumentNullOrEmptyString(string arg)
        {
            return (ArgumentException)this.ThrowHelperError(new ArgumentException(TraceSR.GetString(TraceSR.StringNullOrEmpty), arg));
        }

        internal Exception ThrowHelperFatal(string message, Exception innerException)
        {
            return this.ThrowHelperError(new Exception(message, innerException));
        }

        internal Exception ThrowHelperInvalidOperation(string message)
        {
            return ThrowHelperError(new InvalidOperationException(message));
        }

        internal Exception ThrowHelperCritical(Exception exception)
        {
            return this.ThrowHelper(exception, TraceEventType.Critical);
        }

        internal Exception ThrowHelperError(Exception exception)
        {
            return this.ThrowHelper(exception, TraceEventType.Error);
        }

        internal Exception ThrowHelperWarning(Exception exception)
        {
            return this.ThrowHelper(exception, TraceEventType.Warning);
        }

        internal Exception ThrowHelperXml(XmlReader reader, string message)
        {
            return this.ThrowHelperXml(reader, message, null);
        }

        internal Exception ThrowHelperXml(XmlReader reader, string message, Exception inner)
        {
            var lineInfo = reader as IXmlLineInfo;
            return this.ThrowHelperError(new XmlException(
                message,
                inner,
                (null != lineInfo) ? lineInfo.LineNumber : 0,
                (null != lineInfo) ? lineInfo.LinePosition : 0));
        }
    }
}