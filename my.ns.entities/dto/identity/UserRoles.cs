using System.ComponentModel.DataAnnotations;

namespace my.ns.entities.dto.identity
{
    using RTYP = Resources.Views.Admin.Users.Resources;

    public class UserRoles
    {
        [Key]
        [Display(Name = "UserRoles_RoleName", ResourceType = typeof(RTYP))]
        public string RoleName { get; set; }
    }
}
