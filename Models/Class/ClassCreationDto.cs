using System.ComponentModel.DataAnnotations;

namespace Attendr.API.Models.Class
{
    public class ClassCreationDto
    {
        [Required]
        [MaxLength('3', ErrorMessage = "Year name must be 3 characters in length. Example: 078")]
        public string Year { get; set; } = string.Empty;
        [Required]
        [MaxLength('3', ErrorMessage = "Department must be 3 characters in length. Example: bct")]
        public string Department { get; set; } = string.Empty;
        [Required]
        [MaxLength('1', ErrorMessage = "Group name can be only one character in length")]
        public string Group { get; set; } = string.Empty;
    }
}
