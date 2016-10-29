using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my.ns.entities.IdentityConfig
{
    // Add ApplicationRoleManager to allow the management of Roles
    public class ApplicationRoleManager : RoleManager<IdentityRoleIntPk, int>
    {
        public ApplicationRoleManager(ApplicationRoleStore roleStore)
            : base(roleStore)
        {
        }
    }
}
