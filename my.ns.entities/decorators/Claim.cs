using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using my.ns.entities.IdentityConfig;

namespace my.ns.entities.decorators
{
    using dto.identity;

    public class DecoratedUserClaim : ADecorable, IUserClaim
    {
        public DecoratedUserClaim(ApplicationUserManager userManager) : base(userManager) { }
        private string _claimType;

        #region get-set
        public string UserName
        {
            get
            {
                return UserNameOrId ?? (component as UserClaim).UserName;
            }
            set
            {
                (component as UserClaim).UserName = UserNameOrId = value;
            }
        }
        public string ClaimType
        {
            get
            {
                return _claimType ?? (component as UserClaim).ClaimType;
            }
            set
            {
                if ((component as UserClaim) != null)
                    (component as UserClaim).ClaimType = value;
                _claimType = value;
            }
        }
        public string ClaimValue
        {
            get
            {
                return (component as UserClaim).ClaimValue;
            }
            set
            {
                (component as UserClaim).ClaimValue = value;
            }
        }
        #endregion


        public override dynamic Decorate()
        {
            if (AppUser == null || _claimType == null)
            {
                throw new ArgumentException("None or unknown argument specified. Make sure to set both UserNameOrId and ClaimType before issuing Decorate()");
            }
            UserClaim claim = new UserClaim() { UserName = AppUser.UserName };
            foreach (var iclaim in AppUser.Claims)
            {
                if (iclaim.ClaimType.ToLower().Equals(_claimType))
                { claim.ClaimType = _claimType; claim.ClaimValue = iclaim.ClaimValue; break; }
            }
            this.component = claim;
            return claim;
        }
    }
}
