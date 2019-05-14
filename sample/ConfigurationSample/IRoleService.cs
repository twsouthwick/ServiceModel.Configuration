using System.Collections.Generic;
using System.ServiceModel;

namespace ConfigurationSample
{
    [ServiceContract]
    public interface IRoleService
    {
        [OperationContract]
        IEnumerable<string> GetRoles(string userId);
    }
}
