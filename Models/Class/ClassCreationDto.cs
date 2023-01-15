using System.ComponentModel.DataAnnotations;

namespace Attendr.API.Models.Class
{
    public class ClassCreationDto
    {
        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Year name must be 3 characters in length. Example: 078")]
        public string Year { get; set; } = string.Empty;
        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Department must be 3 characters in length. Example: bct")]
        public string Department { get; set; } = string.Empty;
        [Required]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "Group name must be only one character in length")]
        public string Group { get; set; } = string.Empty;
    }
}
