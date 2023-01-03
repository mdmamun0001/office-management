using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Zdm_management.ViewModel
{
    public class PasswordResetViewModel
    {
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string NewPassword { get; set; }
        [Required]
        [PasswordPropertyText]
        [Compare("NewPassword", ErrorMessage = "Both Password and Confirm Password Must be Same")]
        [DisplayName("Confirm Password")]
        public string NewConfirmPassword { get; set; }
    }
}
