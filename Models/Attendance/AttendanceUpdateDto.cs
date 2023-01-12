using Attendr.API.Models.Class;
using Attendr.API.Models.Teacher;

namespace Attendr.API.Models.Attendance
{
    public class AttendanceUpdateDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid ClassId { get; set; }
        public Guid TeacherId { get; set; }
        public List<AttendanceReportUpdateDto> AttendanceReports { get; set; } = new List<AttendanceReportUpdateDto>();
    }
}
