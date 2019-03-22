using System;

namespace ServiceModel.Configuration
{
    public class ServiceConfigurationException : Exception
    {
        public ServiceConfigurationException(string message)
            : base(message)
        {
        }
    }
}
