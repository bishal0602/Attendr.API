using System.ComponentModel.DataAnnotations;

namespace Attendr.API.Models.Class
{
    public class ClassCreationDto
    {
        [Required]
        public string Year { get; set; } = string.Empty;
        [Required]
        public string Department { get; set; } = string.Empty;
        [Required]
        [MaxLength('1', ErrorMessage = "Group name can be only one character in length")]
        public string Group { get; set; } = string.Empty;
    }
}
