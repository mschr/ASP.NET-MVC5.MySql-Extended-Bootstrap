using my.ns.entities.decorators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my.ns.entities.dto.identity
{
    public class UserAndClaims : IDecorable
    {
        [Key]
        [Display(Name = "User_UserName")]
        public string UserName { get; set; }
        [Display(Name = "User_Claims")]
        public List<UserClaim> Claims { get; set; }
    }
}
