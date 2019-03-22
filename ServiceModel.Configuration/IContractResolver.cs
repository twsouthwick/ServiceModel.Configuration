using System;

namespace ServiceModel.Configuration
{
    public interface IContractResolver
    {
        bool TryResolve(string name, out Type type);
    }
}
