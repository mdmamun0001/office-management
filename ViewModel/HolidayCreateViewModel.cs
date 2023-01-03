using System.ComponentModel.DataAnnotations;

namespace Zdm_management.ViewModel
{
    public class HolidayCreateViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime HoldayDate { get; set; }
        public string? ShortDescription { get; set; }
    }
}
