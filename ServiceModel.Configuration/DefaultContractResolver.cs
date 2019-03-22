using System;
using System.Linq;

namespace ServiceModel.Configuration
{
    internal class DefaultContractResolver : IContractResolver
    {
        public bool TryResolve(string name, out Type type)
        {
            var items = AppDomain.CurrentDomain.GetAssemblies().OrderBy(t => t.FullName).ToList();
            var mine = items.First(t => t.GetName().Name== "ServiceModel.Configuration.Xml.Tests");
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetType(name) is Type found)
                {
                    type = found;
                    return true;
                }
            }

            type = null;
            return false;
        }
    }
}
