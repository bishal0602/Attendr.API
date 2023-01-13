using Attendr.API.Entities;

namespace Attendr.API.Services.Models
{
    public class StudentAttendanceReport
    {
        public Student Student { get; set; }
        public int TotalClassAttended { get; set; }
        public int TotalClass { get; set; }
    }
}
