using my.ns.entities.decorators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace my.ns.entities.dto.identity
{
    public class UserAndRoles : IDecorable
    {
        [Key]
        [Display(Name = "User_UserName")]
        public string UserName { get; set; }
        [Display(Name = "User_Roles")]
        public List<UserRole> Roles { get; set; }
    }
}
