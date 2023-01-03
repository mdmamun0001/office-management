using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Zdm_management.ViewModel.CustomValidation;

namespace Zdm_management.ViewModel
{
    public class UserCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public int IdCardNumber { get; set; }
        [Required]
        public string JobTitle { get; set; }
        public DateTime? BirthDay { get; set; }
        [Required]
        public string Gender { get; set; }
        public string? Address { get; set; }
        [DataType(DataType.Upload)]
        [MaxFileSize(1 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".gif", ".svg" })]
        public IFormFile? ProfileImage { get; set; }
        [DataType(DataType.Upload)]
        [MaxFileSize(1 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".gif", ".svg" })]
        public IFormFile? CoverImage { get; set; }
       
        public string? BloodGroup { get; set; }
        [Required]
        [PasswordPropertyText]
        public  string Password { get; set; }
        [Required]
        [PasswordPropertyText]
        [Compare("Password", ErrorMessage = "Both Password and Confirm Password Must be Same")]
        [DisplayName("Confirm Password")]
        public  string ConfirmPassword { get; set; }
    }
}
