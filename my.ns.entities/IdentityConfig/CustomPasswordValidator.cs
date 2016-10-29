using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my.ns.entities.IdentityConfig
{
    public static class PasswordValidatorCodes
    {
        public const string ErrorCodePrefix = "CustomPassword";
        public const string PasswordTooShort = ErrorCodePrefix + "TooShort";
        public const string PasswordRequireNonLetterOrDigit = ErrorCodePrefix + "RequireNonLetterOrDigit";
        public const string PasswordRequireDigit = ErrorCodePrefix + "RequireDigit";
        public const string PasswordRequireLower = ErrorCodePrefix + "RequireLower";
        public const string PasswordRequireUpper = ErrorCodePrefix + "RequireUpper";

        public static string GetLocalizedMessageForCode(string code)
        {
            switch (code)
            {
                case PasswordTooShort:
                    return string.Format(Resources.Resources.ValidationPasswordTooShort, CustomPasswordValidator.RequiredPasswordLength);
                case PasswordRequireNonLetterOrDigit:
                    return Resources.Resources.ValidationPasswordRequireNonLetterOrDigit;
                case PasswordRequireDigit:
                    return Resources.Resources.ValidationPasswordRequireDigit;
                case PasswordRequireLower:
                    return Resources.Resources.ValidationPasswordRequireLower;
                case PasswordRequireUpper:
                    return Resources.Resources.ValidationPasswordRequireUpper;
                default:
                    throw new ArgumentException("code");
            }
        }
    }

    public class CustomPasswordValidator : PasswordValidator
    {
        public const int RequiredPasswordLength = 6;

        public CustomPasswordValidator()
        {
            RequiredLength = RequiredPasswordLength;
            RequireNonLetterOrDigit = false;
            RequireDigit = true;
            RequireLowercase = true;
            RequireUppercase = false;
        }

        public override Task<IdentityResult> ValidateAsync(string item)
        {
            if (item == null) throw new ArgumentNullException("item");
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(item) || item.Length < RequiredLength)
            {
                errors.Add(PasswordValidatorCodes.PasswordTooShort);
            }

            if (RequireNonLetterOrDigit && item.All(IsLetterOrDigit))
            {
                errors.Add(PasswordValidatorCodes.PasswordRequireNonLetterOrDigit);
            }

            if (RequireDigit && item.All(c => !IsDigit(c)))
            {
                errors.Add(PasswordValidatorCodes.PasswordRequireDigit);
            }

            if (RequireLowercase && item.All(c => !IsLower(c)))
            {
                errors.Add(PasswordValidatorCodes.PasswordRequireLower);
            }

            if (RequireUppercase && item.All(c => !IsUpper(c)))
            {
                errors.Add(PasswordValidatorCodes.PasswordRequireUpper);
            }

            return Task.FromResult(errors.Count == 0
                ? IdentityResult.Success
                : new IdentityResult(errors));
        }
    }
}
