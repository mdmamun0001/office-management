using System.ComponentModel.DataAnnotations;

namespace Zdm_management.ViewModel
{
    public class RoleCreateViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
