using Attendr.API.Models.Student;

namespace Attendr.API.Models.Attendance
{
    public class AttendanceReportDto
    {
        public Guid Id { get; set; }
        public StudentDto Student { get; set; }
        public Guid StudentId { get; set; }
        public bool? isPresent { get; set; }
        public AttendanceDto Attendance { get; set; }
        public Guid AttendanceId { get; set; }
    }
}
