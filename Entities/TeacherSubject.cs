namespace Attendr.API.Entities
{
    public class TeacherSubject
    {
        public Guid Id { get; set; }
        public string TeacherName { get; set; }
        public string TeacherShortName { get; set; }
        public string SubjectName { get; set; }
        public List<Period> Periods { get; set; }
        public Attendance Attendance { get; set; }
    }
}
