using System;
using System.ServiceModel.Description;

namespace ServiceModel.Configuration
{
    public interface IContractResolver
    {
        Type ResolveContract(string name);

        ContractDescription ResolveDescription(Type type);
    }
}
