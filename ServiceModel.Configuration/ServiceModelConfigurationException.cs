using System;

namespace ServiceModel.Configuration
{
    public class ServiceModelConfigurationException : Exception
    {
        public ServiceModelConfigurationException(string message)
            : base(message)
        {
        }
    }
}
