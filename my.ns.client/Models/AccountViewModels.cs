using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace my.ns.client.Models
{
    using RTYP = Resources.Resources;

    public class ExternalLoginConfirmationViewModel
    {
        #region #Validate_annotations
        [Display(Name = "Email", ResourceType = typeof(RTYP))]
        [Required(ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputFieldRequired")]
        [StringLength(128, ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputStringLengthValidationError")]
        #endregion
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        #region #Validate_annotations
        [Display(Name = "Provider", ResourceType = typeof(RTYP))]
        [Required(ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputFieldRequired")]
        #endregion
        public string Provider { get; set; }
        #region #Validate_annotations
        [Display(Name = "VerifyCode", ResourceType = typeof(RTYP))]
        [Required(ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputFieldRequired")]
        #endregion
        public string Code { get; set; }
        public string ReturnUrl { get; set; }
        #region #Validate_annotations
        [Display(Name = "RememberThisBrowser", ResourceType = typeof(RTYP))]
        #endregion
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        #region #Validate_annotations
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(RTYP))]
        [Required(ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputFieldRequired")]
        [StringLength(128, ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputStringLengthValidationError")]
        #endregion
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        #region #Validate_annotations
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(RTYP))]
        [Required(ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputFieldRequired")]
        [StringLength(128, ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputStringLengthValidationError")]
        #endregion
        public string Email { get; set; }
        #region #Validate_annotations
        [Required(ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputFieldRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        #endregion
        public string Password { get; set; }
        #region #Validate_annotations
        [Display(Name = "RememberMe", ResourceType = typeof(RTYP))]
        #endregion
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        #region #Validate_annotations
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(RTYP))]
        [Required(ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputFieldRequired")]
        [StringLength(128, ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputStringLengthValidationError")]
        #endregion
        public string Email { get; set; }
        #region #Validate_annotations
        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputStringLengthValidationError", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        #endregion
        public string Password { get; set; }
        #region #Validate_annotations
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(RTYP))]
        [Compare("Password", ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "ConfirmPasswordMisMatch")]
        #endregion
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        #region #Validate_annotations
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(RTYP))]
        [Required(ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputFieldRequired")]
        [StringLength(128, ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputStringLengthValidationError")]
        #endregion
        public string Email { get; set; }
        #region #Validate_annotations
        [Required(ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputFieldRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputStringLengthValidationError", MinimumLength = 6)]
        [DataType(DataType.Password)]
        #endregion
        [Display(Name = "Password")]
        public string Password { get; set; }
        #region #Validate_annotations
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(RTYP))]
        [Compare("Password", ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "ConfirmPasswordMisMatch")]
        #endregion
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        #region #Validate_annotations
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(RTYP))]
        [Required(ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputFieldRequired")]
        [StringLength(128, ErrorMessageResourceType = typeof(RTYP), ErrorMessageResourceName = "InputStringLengthValidationError")]
        #endregion
        public string Email { get; set; }
    }
}
