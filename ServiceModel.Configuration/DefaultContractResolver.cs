using System;
using System.Linq;
using System.ServiceModel.Description;

namespace ServiceModel.Configuration
{
    internal class DefaultContractResolver : IContractResolver
    {
        public virtual ContractDescription ResolveDescription(Type type) => ContractDescription.GetContract(type);

        public virtual Type ResolveContract(string name)
        {
            var items = AppDomain.CurrentDomain.GetAssemblies().OrderBy(t => t.FullName).ToList();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetType(name) is Type found)
                {
                    return found;
                }
            }

            throw new ServiceModelConfigurationException($"Could not resolve contract '{name}'");
        }
    }
}
