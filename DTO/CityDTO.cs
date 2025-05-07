using System.ComponentModel.DataAnnotations;

namespace AsadTutorialWebAPI.DTO
{
    public class CityDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Name field is mandatory")]
        [StringLength(25,MinimumLength = 2)]
        [RegularExpression(".*[a-zA-Z]+.*", ErrorMessage ="Only numerics are not allowed")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Country name is required")]
        public string Country { get; set; }
                     
    }
}
