using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my.ns.entities.IdentityConfig
{
    public class UserLoginIntPk : IdentityUserLogin<int>
    { }

    public class UserRoleIntPk : IdentityUserRole<int>
    { }

    public class UserClaimIntPk : IdentityUserClaim<int>
    { }
    public class IdentityRoleIntPk : IdentityRole<int, UserRoleIntPk>
    {
        public IdentityRoleIntPk() { }
        public IdentityRoleIntPk(string name) { Name = name; }
    }

    public class ApplicationUserStore : UserStore<ApplicationUser, IdentityRoleIntPk, int,
    UserLoginIntPk, UserRoleIntPk, UserClaimIntPk>
    {
        public ApplicationUserStore(DbContexts.IdentityDb context)
          : base(context)
        {
        }
    }

    public class ApplicationRoleStore : RoleStore<IdentityRoleIntPk, int, UserRoleIntPk>
    {
        public ApplicationRoleStore(DbContexts.IdentityDb context)
           : base(context)
        {
        }
    }
}
