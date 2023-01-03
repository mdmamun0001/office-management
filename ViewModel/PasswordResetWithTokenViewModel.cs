using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Zdm_management.ViewModel
{
    public class PasswordResetWithTokenViewModel : PasswordResetViewModel
    {
        [Required]
        public string Token { get; set; }
    }
}
