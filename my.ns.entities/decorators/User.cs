using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace my.ns.entities.decorators
{
    using dto.identity;
    using IdentityConfig;
    public class DecoratedUser : ADecorable, IUser
    {
        public DecoratedUser(ApplicationUserManager userManager) : base(userManager) { }
        #region get-set
        public string UserName
        {
            get
            {
                return (component as User).UserName;
            }
            set
            {
                (component as User).UserName = value;
            }
        }
        public string Email
        {
            get
            {
                return (component as User).Email;
            }
            set
            {
                (component as User).Email = value;
            }
        }
        public string Password
        {
            get
            {
                return (component as User).Password;
            }
            set
            {
                (component as User).Password = value;
            }
        }
        public DateTime? LockoutEndDateUtc
        {
            get
            {
                return (component as User).LockoutEndDateUtc;
            }
            set
            {
                (component as User).LockoutEndDateUtc = value;
            }
        }
        public int AccessFailedCount
        {
            get
            {
                return (component as User).AccessFailedCount;
            }
            set
            {
                (component as User).AccessFailedCount = value;
            }
        }
        public string PhoneNumber
        {
            get
            {
                return (component as User).PhoneNumber;
            }
            set
            {
                (component as User).PhoneNumber = value;
            }
        }
        public IEnumerable<UserRoles> Roles
        {
            get
            {
                return (component as User).Roles;
            }
            set
            {
                (component as User).Roles = value;
            }
        }
        #endregion

        public override dynamic Decorate()
        {
            if (AppUser == null)
            {
                throw new ArgumentException("None or unknown argument specified. Make sure to set both UserNameOrId and ClaimType before issuing Decorate()");
            }

            User user = new User()
            {
                UserName = AppUser.UserName,
                Email = AppUser.Email,
                LockoutEndDateUtc = AppUser.LockoutEndDateUtc,
                AccessFailedCount = AppUser.AccessFailedCount,
                PhoneNumber = AppUser.PhoneNumber,
                Password = AppUser.PasswordHash,
                UserClaims = AppUser.Claims
                    .Where(c => ApplicationUser.acceptedClaims.Contains(c.ClaimType))
                    .Select(c => new UserClaims { ClaimType = c.ClaimType, ClaimValue = c.ClaimValue }),
                Roles = UserManager.GetRoles(AppUser.Id)
                    .Select(r => new UserRoles { RoleName = r })
            };
            this.component = user;
            return user;
        }
    }
}
