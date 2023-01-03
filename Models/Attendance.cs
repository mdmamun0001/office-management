
using System.ComponentModel.DataAnnotations;
namespace Zdm_management.Models
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        [Required]
        public string ApplicationUserID { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
