using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Zdm_management.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int IdCardNumber { get; set; }
        public string? JobTitle { get; set; }
        public DateTime? BirthDay { get; set; }

        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? ProfileImage { get; set; }
        public string? CoverImage { get; set; }
        public string? BloodGroup { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public List<Attendance>? Attendances { get; set; }
        public List<Leave>? Leaves { get; set; }
    }
}
