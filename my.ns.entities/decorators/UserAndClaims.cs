using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using my.ns.entities.IdentityConfig;

namespace my.ns.entities.decorators
{
    using dto.identity;
    public class DecoratedUserAndClaims : ADecorable, IUserAndClaims
    {
        public DecoratedUserAndClaims(ApplicationUserManager userManager) : base(userManager) { }
        #region get-set
        public string UserName
        {
            get
            {
                return UserNameOrId ?? (component as UserAndClaims).UserName;
            }
            set
            {
                if (null != (component as UserAndClaims))
                    (component as UserAndClaims).UserName = value;
                UserNameOrId = value;
            }
        }
        public List<UserClaim> Claims
        {
            get
            {
                return (component as UserAndClaims).Claims;
            }
            set
            {
                (component as UserAndClaims).Claims = value;
            }
        }
        #endregion

        public override dynamic Decorate()
        {
            if (AppUser == null)
            {
                throw new ArgumentException("None or unknown argument UserNameOrId specified. Make sure to set before issuing Decorate()");
            }
            UserAndClaims userAndClaim = new UserAndClaims() { UserName = AppUser.UserName, Claims = new List<UserClaim>() };
            foreach (var identityClaim in AppUser.Claims)
            {
                userAndClaim.Claims.Add(new UserClaim
                {

                    UserName = AppUser.UserName,
                    UserId = AppUser.Id,
                    ClaimType = identityClaim.ClaimType,
                    ClaimValue = identityClaim.ClaimValue
                });
            }
            this.component = userAndClaim;
            return userAndClaim;
        }
    }
}
