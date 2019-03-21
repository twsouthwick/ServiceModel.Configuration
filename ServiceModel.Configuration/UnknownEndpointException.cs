using System;

namespace ServiceModel.Configuration
{
    public class UnknownEndpointException : Exception
    {
        public UnknownEndpointException(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
