using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using my.ns.entities.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace my.ns.entities.IdentityConfig
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, UserLoginIntPk, UserRoleIntPk, UserClaimIntPk>
    {
        public static List<string> acceptedClaims = new List<string> {
                ClaimTypes.Country,
                ClaimTypes.DateOfBirth,
                ClaimTypes.Gender,
                ClaimTypes.GivenName,
                ClaimTypes.HomePhone,
                ClaimTypes.MobilePhone,
                ClaimTypes.PostalCode,
                ClaimTypes.StreetAddress,
                ClaimTypes.Locality
        };

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            using (var db = new IdentityDb())
            {
                db.Database.Log = (c) => { System.Diagnostics.Debug.WriteLine(c); };

                foreach (var iclaim in userIdentity.Claims)
                {
                    if (acceptedClaims.Contains(iclaim.Type))
                    {
                        var usr = await manager.FindByIdAsync(this.Id);
                        var claim = usr.Claims.Where(c => c.ClaimType == iclaim.Type).FirstOrDefault();

                        if (claim == null)
                        {
                            usr.Claims
                                .Add(new UserClaimIntPk
                                {
                                    ClaimType = iclaim.Type,
                                    ClaimValue = iclaim.Value,
                                    UserId = this.Id
                                });
                        }
                        else
                        {
                            if (claim.ClaimValue != iclaim.Value)
                            {
                                claim.ClaimValue = iclaim.Value;
                                db.Entry(claim).State = System.Data.Entity.EntityState.Modified;
                            }

                        }

                    }
                }
                db.SaveChanges();
            }
            return userIdentity;
        }
    }
}
