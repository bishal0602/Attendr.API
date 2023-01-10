namespace Attendr.API.Entities
{
    public class Period
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public Routine Routine { get; set; }
        public Guid RoutineId { get; set; }
        public TeacherSubject TeacherSubject { get; set; }
        public Guid TeacherSubjectId { get; set; }
    }
}
