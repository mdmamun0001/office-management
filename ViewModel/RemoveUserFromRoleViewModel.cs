using System.ComponentModel.DataAnnotations;

namespace Zdm_management.ViewModel
{
    public class RemoveUserFromRoleViewModel
    {

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
