using System.ComponentModel.DataAnnotations;

namespace my.ns.entities.dto.identity
{
    using RTYP = Resources.Views.Admin.Users.Resources;

        public class UserRole
    {
        [Key]
        [Display(Name = "User_UserName", ResourceType = typeof(RTYP))]
        public string UserName { get; set; }
        [Display(Name = "UserRoles_RoleName", ResourceType = typeof(RTYP))]
        public string RoleName { get; set; }
    }
}
