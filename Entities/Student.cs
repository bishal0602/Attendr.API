using System.ComponentModel.DataAnnotations;

namespace Attendr.API.Entities
{
    public class Student
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; } = string.Empty;
        public Guid ClassId { get; set; }
        public Class Class { get; set; }

        public List<AttendanceReport> AttendanceReports { get; set; } = new List<AttendanceReport>();

    }
}
