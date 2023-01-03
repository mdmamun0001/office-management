using System.ComponentModel.DataAnnotations;

namespace Zdm_management.Models
{
    public class Leave
    {
        [Key]
        public int id { get; set; }
        public string? Reason { get; set; }
        [Required]
        public DateTime Leavedate { get; set; }
        public string? Type { get; set; }
        [Required]
        public string ApplicationUserID { get; set; }
    }
}
