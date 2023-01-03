using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
namespace Zdm_management.Models
{
    public class Holiday
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime HolidayDate { get; set; }
        public string? ShortDescription { get; set; }
    }
}
