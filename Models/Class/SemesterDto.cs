

using Attendr.API.Models.Routine;
using Attendr.API.Models.Teacher;

namespace Attendr.API.Models.Class
{
    public class SemesterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ClassId { get; set; }
        public List<RoutineDto> Routines { get; set; } = new List<RoutineDto>();
        public List<TeacherDto> Teachers { get; set; } = new List<TeacherDto>();
    }
}
