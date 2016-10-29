using my.ns.entities.DbContexts;
using my.ns.entities.IdentityConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Data.Common;


namespace my.ns.client.Helpers
{
    public static class IdentityExtensions
    {
        private static readonly ApplicationUserStore store = new ApplicationUserStore(new IdentityDb());
        private static readonly ApplicationUserManager manager = new ApplicationUserManager(store);

        public static int GetUserIdInteger(this ClaimsIdentity identity)
        {
            var usr = manager.FindByName(identity.Name);
            return (usr != null) ? usr.Id : 0;
        }
        public static ApplicationUser GetApplicationUser(this IIdentity identity)
        {
           
            return store.Users.Where(u => u.UserName == identity.Name).FirstOrDefault();
        }
        public static string GetClaimValue(this ApplicationUser user, string claimType)
        {
            var claim = user.Claims.Where(c => c.ClaimType == claimType).FirstOrDefault();
            return (claim != null) ? claim.ClaimValue : "";
        }
    }
}