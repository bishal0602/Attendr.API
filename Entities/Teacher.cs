namespace Attendr.API.Entities
{
    public class Teacher
    {
        public Guid Id { get; set; }
        public string TeacherName { get; set; }
        public string TeacherShortName { get; set; }
        public string SubjectName { get; set; }
        public List<Period> Periods { get; set; }
        public List<Attendance> Attendances { get; set; }
        public Semester Semester { get; set; }
        public Guid SemesterId { get; set; }
    }
}
