using System.ComponentModel.DataAnnotations;

namespace Zdm_management.Models
{
    public class IpWishList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string IpAddress { get; set; }
    }
}
