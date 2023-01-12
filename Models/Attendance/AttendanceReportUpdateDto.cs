namespace Attendr.API.Models.Attendance
{
    public class AttendanceReportUpdateDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public bool? isPresent { get; set; }
        public Guid AttendanceId { get; set; }
    }
}
