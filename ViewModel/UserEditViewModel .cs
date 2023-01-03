using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Zdm_management.ViewModel.CustomValidation;

namespace Zdm_management.ViewModel
{
    public class UserEditViewModel : UserCreateViewModel
    {
        [Required]
        public string Id { get; set; }
        public string? ExistingProfileImage { get; set; }
        public string? ExistingCoverImage { get; set; }
  
        [PasswordPropertyText]
        public  string? Password { get; set; }
   
        [PasswordPropertyText]
        [Compare("Password", ErrorMessage = "Both Password and Confirm Password Must be Same")]
        [DisplayName("Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}
