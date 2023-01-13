using Attendr.API.Models.Student;
using Attendr.API.Models.Teacher;

namespace Attendr.API.Models.Attendance
{
    public class TeacherAttendanceReportDto
    {
        public TeacherDto Teacher { get; set; }
        public List<StudentAttendanceReportDto> AttendanceReports { get; set; } = new List<StudentAttendanceReportDto>();
        public int TotalClass { get; set; }
    }
}
