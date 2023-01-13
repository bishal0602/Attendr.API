namespace Attendr.API.Entities
{
    public class AttendanceReport
    {
        public Guid Id { get; set; }
        public Student Student { get; set; }
        public Guid StudentId { get; set; }
        public bool? IsPresent { get; set; }
        public Guid TeacherId { get; set; }
        public Attendance Attendance { get; set; }
        public Guid AttendanceId { get; set; }
    }
}
