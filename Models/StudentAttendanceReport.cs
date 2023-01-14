namespace Attendr.API.Models
{
    public class StudentAttendanceReport
    {
        public Entities.Student Student { get; set; }
        public int TotalClassAttended { get; set; }
        public int TotalClass { get; set; }
    }
}
