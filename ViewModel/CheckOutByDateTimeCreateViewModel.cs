using System.ComponentModel.DataAnnotations;

namespace Zdm_management.ViewModel
{
    public class CheckOutByDateTimeCreateViewModel
    {
        [Required]
        public string UserEmail { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CheckOutTime { get; set; }
    }
}
