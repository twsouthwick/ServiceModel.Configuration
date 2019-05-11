using System.Collections.Generic;
using System.Linq;

namespace WcfServer
{
    public class RoleServiceImpl : IRoleService
    {
        public IEnumerable<string> GetRoles(string userId)
        {
            switch (userId)
            {
                case "user1":
                    return new[] { "role1" };
                case "user2":
                    return new[] { "role2" };
                default:
                    return Enumerable.Empty<string>();
            }
        }
    }
}
