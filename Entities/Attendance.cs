namespace Attendr.API.Entities
{
    public class Attendance
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Class Class { get; set; }
        public Guid ClassId { get; set; }
        public Teacher Teacher { get; set; }
        public Guid TeacherId { get; set; }
        public List<AttendanceReport> AttendanceReports { get; set; } = new List<AttendanceReport>();
    }
}
