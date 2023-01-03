using System.ComponentModel.DataAnnotations;

namespace Zdm_management.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
