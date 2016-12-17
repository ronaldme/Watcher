using System.ComponentModel.DataAnnotations;
using Watcher.Web.Translations;

namespace Watcher.Web.ViewModels
{
    public class Account
    {
        [EmailAddress(ErrorMessageResourceName = "EmailNotValid", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resources))]
        [Display(ResourceType = typeof(Resources), Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginConfirmationViewModel : Account
    {
    }

    public class ForgotPasswordViewModel : Account
    {
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel : Account
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel : Account
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceName = "DoNotMatch", ErrorMessage = null, ErrorMessageResourceType = typeof(Resources))]
        public string ConfirmPassword { get; set; }
    }
}
