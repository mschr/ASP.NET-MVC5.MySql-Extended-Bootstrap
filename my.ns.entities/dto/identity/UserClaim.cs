using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my.ns.entities.dto.identity
{
    using decorators;
    using RTYP = Resources.Views.Admin.Users.Resources;

    public class UserClaim : UserClaims, IDecorable
    {
        [Display(Name = "User_UserName", ResourceType = typeof(RTYP))]
        public string UserName { get; set; }

        /*
        //
        // Summary:
        //     Claim type
        [Display(Name = "UserClaim_ClaimType", ResourceType = typeof(RTYP))]
        public  string ClaimType { get; set; }
        //
        // Summary:
        //     Claim value
        [Display(Name = "UserClaim_ClaimValue", ResourceType = typeof(RTYP))]

        public  string ClaimValue { get; set; }
        */

        //
        // Summary:
        //     User Id for the user who owns this login
        public int UserId { get; set; }
    }
}
