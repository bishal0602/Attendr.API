

using Attendr.API.Models.Teacher;

namespace Attendr.API.Models.Class
{
    public class SemesterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ClassId { get; set; }
        //public List<Routine> Routines { get; set; } = new List<Routine>();
        public List<TeacherDto> Teachers { get; set; } = new List<TeacherDto>();
    }
}
