using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my.ns.entities.decorators
{
    using dto.identity;
    using IdentityConfig;
    public class DecoratedUserAndRoles : ADecorable, IUserAndRoles
    {

        public DecoratedUserAndRoles(ApplicationUserManager userManager) : base(userManager) { }

        #region get-set
        public string UserName
        {
            get
            {
                return (component as UserAndRoles).UserName;
            }
            set
            {
                (component as UserAndRoles).UserName = value;
            }
        }
        public List<UserRole> Roles
        {
            get
            {
                return (component as UserAndRoles).Roles;
            }
            set
            {
                (component as UserAndRoles).Roles = value;
            }
        }
        #endregion

        /// <summary>
        /// Decorates all available variables on UserAndRoles, for the given username or id/applicationUser on construct
        /// </summary>
        /// <returns>UserAndRoles</returns>
        public override dynamic Decorate()
        {
            if (AppUser == null || UserNameOrId == null)
            {
                throw new ArgumentException("None or unknown argument UserNameOrId specified. Make sure to set before issuing Decorate()");
            }
            UserAndRoles unr = new UserAndRoles()
            {
                UserName = AppUser.UserName,
                Roles = new List<UserRole>()
            };
            foreach (var roleName in UserManager.GetRoles(AppUser.Id))
            {
                unr.Roles.Add(new UserRole
                {
                    UserName = AppUser.UserName,
                    RoleName = roleName
                });
            }
            this.component = unr;
            return unr;
        }
    }
}
