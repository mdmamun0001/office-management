using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Zdm_management.ViewModel
{
    public class UserLoginViewModel
    {
        [Required]
        [EmailAddress]
        [DisplayName("User Email")]
        public string UserEmail { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        [DisplayName("Remember me")]
        public Boolean RememberMe { get; set; }
    }
}
