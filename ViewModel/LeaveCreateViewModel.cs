using System.ComponentModel.DataAnnotations;

namespace Zdm_management.ViewModel
{
    public class LeaveCreateViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime LeaveDate { get; set; }
        public string? Reason { get; set; }
        public string? Type { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
