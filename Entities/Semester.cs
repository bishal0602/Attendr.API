namespace Attendr.API.Entities
{
    public class Semester
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Class Class { get; set; }
        public Guid ClassId { get; set; }
        //public SemesterRoutine SemesterRoutine { get; set; }
        //public Guid SemesterRoutineId { get; set; }
        public List<Routine> Routines { get; set; } = new List<Routine>();
        public List<Teacher> Teachers { get; set; } = new List<Teacher>();
    }
}
