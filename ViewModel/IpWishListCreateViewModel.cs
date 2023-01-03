using System.ComponentModel.DataAnnotations;
using Zdm_management.ViewModel.CustomValidation;

namespace Zdm_management.ViewModel
{
    public class IpWishListCreateViewModel
    {
        [Required]
        [IPAddressValidation()]
        [RegularExpression(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b",
         ErrorMessage = "In Valid Input.")]
        public string IpAddress { get; set; }
    }
}
