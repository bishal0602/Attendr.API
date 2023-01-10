namespace Attendr.API.Entities
{
    public class AttendanceReport
    {
        public Guid Id { get; set; }
        public Student Student { get; set; }
        public Guid StudentId { get; set; }
        public bool isPresent { get; set; }
        public Attendance Attendance { get; set; }
        public Guid AttendanceId { get; set; }
    }
}
