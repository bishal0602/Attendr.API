using Attendr.API.Models.Teacher;

namespace Attendr.API.Models.Routine
{
    public class PeriodDto
    {
        public Guid Id { get; set; }
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public RoutineDto Routine { get; set; }
        public Guid RoutineId { get; set; }
        public TeacherDto Teacher { get; set; }
        public Guid TeacherId { get; set; }
    }
}
