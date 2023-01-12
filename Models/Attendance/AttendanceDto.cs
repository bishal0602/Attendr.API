using Attendr.API.Models.Class;
using Attendr.API.Models.Teacher;

namespace Attendr.API.Models.Attendance
{
    public class AttendanceDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public ClassDto Class { get; set; }
        public Guid ClassId { get; set; }
        public TeacherDto Teacher { get; set; }
        public Guid TeacherId { get; set; }
        public List<AttendanceReportDto> AttendanceReports { get; set; } = new List<AttendanceReportDto>();
    }
}
