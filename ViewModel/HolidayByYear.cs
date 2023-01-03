using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Zdm_management.ViewModel
{
    public class HolidayByYear
    {

        [Required]
        [IntegerValidator]
        public int Year  {get; set;}
    }
}
