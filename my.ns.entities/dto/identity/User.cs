using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using my.ns.entities.decorators;

namespace my.ns.entities.dto.identity
{
    using RVAL = Resources.Resources;
    using RTYP = Resources.Views.Admin.Users.Resources;
    public class User : IDecorable
    {
        [Key]
        [Display(Name = "User_UserName", ResourceType = typeof(RTYP))]
        public string UserName { get; set; }
        [Display(Name = "User_Email", ResourceType = typeof(RTYP))]
        #region #Validate_annotations
        [Required(ErrorMessageResourceType = typeof(RVAL), ErrorMessageResourceName = "InputFieldRequired")]
        [StringLength(128, ErrorMessageResourceType = typeof(RVAL), ErrorMessageResourceName = "InputStringLengthValidationError")]
        #endregion
        public string Email { get; set; }
        [Display(Name = "User_Password", ResourceType = typeof(RTYP))]
        public string Password { get; set; }
        [Display(Name = "User_LockoutEndDateUtc", ResourceType = typeof(RTYP))]
        public DateTime? LockoutEndDateUtc { get; set; }
        [Display(Name = "User_AccessFailedCount", ResourceType = typeof(RTYP))]
        public int AccessFailedCount { get; set; }
        [Display(Name = "User_PhoneNumber", ResourceType = typeof(RTYP))]
        public string PhoneNumber { get; set; }
        [Display(Name = "User_Roles", ResourceType = typeof(RTYP))]
        public IEnumerable<UserRoles> Roles { get; set; }
        [Display(Name = "UserClaim_ClaimType", ResourceType = typeof(RTYP))]
        public IEnumerable<UserClaims> UserClaims { get; set; }
    }
}
