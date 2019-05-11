using System.Collections.Generic;
using System.ServiceModel;

namespace WcfServer
{
    [ServiceContract]
    public interface IRoleService
    {
        [OperationContract]
        IEnumerable<string> GetRoles(string userId);
    }
}
