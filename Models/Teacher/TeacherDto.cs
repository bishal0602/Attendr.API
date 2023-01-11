using Attendr.API.Models.Class;
using Newtonsoft.Json;

namespace Attendr.API.Models.Teacher
{
    public class TeacherDto
    {
        public Guid Id { get; set; }
        [JsonProperty("name")]
        public string TeacherName { get; set; }
        [JsonProperty("shortname")]
        public string TeacherShortName { get; set; }
        [JsonProperty("subject")]
        public string SubjectName { get; set; }
        //public List<Period> Periods { get; set; }
        //public Attendance Attendance { get; set; }
        //public SemesterDto Semester { get; set; }
        public Guid SemesterId { get; set; }
    }
}
