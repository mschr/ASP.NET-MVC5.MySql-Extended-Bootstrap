using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace my.ns.client.Models
{
    using RTYP = Resources.Resources;

    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Display(Name = "NewPassword", ResourceType = typeof(RTYP))]
        [Required(ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputFieldRequired")]
        [StringLength(100, ErrorMessageResourceName = "ValidationPasswordTooShort", ErrorMessageResourceType = typeof(RTYP), MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(RTYP))]
        [StringLength(100, ErrorMessageResourceName = "ValidationPasswordTooShort", ErrorMessageResourceType = typeof(RTYP), MinimumLength = 6)]
        [Compare("NewPassword", ErrorMessageResourceName = "ConfirmPasswordMisMatch", ErrorMessageResourceType = typeof(RTYP))]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputFieldRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword", ResourceType = typeof(RTYP))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputFieldRequired")]
        [StringLength(100, ErrorMessageResourceName = "ValidationPasswordTooShort", ErrorMessageResourceType = typeof(RTYP), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(RTYP))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(RTYP))]
        [StringLength(100, ErrorMessageResourceName = "ValidationPasswordTooShort", ErrorMessageResourceType = typeof(RTYP), MinimumLength = 6)]
        [Compare("NewPassword", ErrorMessageResourceName = "ConfirmPasswordMisMatch", ErrorMessageResourceType = typeof(RTYP))]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required(ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputFieldRequired")]
        [Phone]
        [Display(Name = "PhoneNumber", ResourceType = typeof(RTYP))]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required(ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputFieldRequired")]
        [Display(Name = "VerifyCode", ResourceType = typeof(RTYP))]
        public string Code { get; set; }

        [Required(ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputFieldRequired")]
        [Phone]
        [Display(Name = "PhoneNumber", ResourceType = typeof(RTYP))]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        [Display(Name = "SelectedProvider", ResourceType = typeof(RTYP))]
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}