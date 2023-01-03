using System.ComponentModel.DataAnnotations;

namespace Zdm_management.ViewModel
{
    public class CheckInByDateTimeCreateViewModel
    {
        [Required]
        public string UserEmail { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CheckInTime { get; set; }
    }
}
