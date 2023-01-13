using Attendr.API.Models.Student;

namespace Attendr.API.Models.Attendance
{
    public class StudentAttendanceReportDto
    {
        public StudentDto Student { get; set; }
        public int TotalClassAttended { get; set; }
        public int TotalClass { get; set; }
    }
}
