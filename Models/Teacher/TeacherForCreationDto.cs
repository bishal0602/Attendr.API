using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Attendr.API.Models.Teacher
{
    public class TeacherForCreationDto
    {
        [Required]
        [MaxLength(100)]
        [JsonProperty("name")]
        public string TeacherName { get; set; } = string.Empty;
        [Required]
        [MaxLength(10)]
        [JsonProperty("shortname")]
        public string TeacherShortName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        [JsonProperty("subject")]
        public string SubjectName { get; set; } = string.Empty;
        [Required]
        [JsonProperty("semester")] // TODO Custom Validation 1st-8th
        public string SemesterName { get; set; } = string.Empty;

    }
}
