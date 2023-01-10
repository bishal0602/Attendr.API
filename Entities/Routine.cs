namespace Attendr.API.Entities
{
    public class Routine
    {
        public Guid Id { get; set; }
        public string WeekDay { get; set; }
        //public SemesterRoutine SemesterRoutine { get; set; }
        //public Guid SemesterRoutineId { get; set; }
        public Semester Semester { get; set; }
        public Guid SemesterId { get; set; }
        public List<Period> Periods { get; set; } = new List<Period>();

    }
}
