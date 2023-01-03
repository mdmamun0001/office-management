using System.ComponentModel.DataAnnotations;

namespace Zdm_management.ViewModel
{
    public class LeaveEditViewModel: LeaveCreateViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserEmail { get; set; }
    }
}
