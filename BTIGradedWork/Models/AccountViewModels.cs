using System.ComponentModel.DataAnnotations;
using BTIGradedWork.Controllers;
using System.ComponentModel;
using System.Collections.Generic;

namespace BTIGradedWork.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
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

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [StringLength(100, ErrorMessage = "{0} can contain up to {1} characters.")]
        [Display(Name = "First name")]
        [RegularExpression(@"^[A-Za-z\-\']*$", ErrorMessage = "{0} must be alphabetic characters with special characters such as hyphen(-) and apostophe(').")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} can contain up to {1} characters.")]
        [RegularExpression(@"^[A-Za-z\-\']*$", ErrorMessage = "{0} must be alphabetic characters with special characters such as hyphen(-) and apostophe(').")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; }
        public StudentAdd Student { get; set; }
    }

    public class RegisterTeacherViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} can contain up to {1} characters.")]
        [RegularExpression(@"^[A-Za-z\-\']*$", ErrorMessage = "{0} must be alphabetic characters with special characters such as hyphen(-) and apostophe(').")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} can contain up to {1} characters.")]
        [RegularExpression(@"^[A-Za-z\-\']*$", ErrorMessage = "{0} must be alphabetic characters with special characters such as hyphen(-) and apostophe(').")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        public TeacherAdd Teacher { get; set; }
    }

    public class EditViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

       [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Email address")]
        public string Email { get; set; }
    }

    public class ApplicationUserBase
    {
        public ApplicationUserBase()
        {
            this.RolesForUser = new List<string>();
        }

        public string Id { get; set; }
        [DisplayName("First name")]
        public string FirstName { get; set; }
        [DisplayName("Last name")]
        public string LastName { get; set; }
        [DisplayName("Email address")]
        public string Email { get; set; }
        [DisplayName("User name")]
        public string UserName { get; set; }

        [DisplayName("Roles for user")]
        public ICollection<string> RolesForUser { get; set; }
    }
}
