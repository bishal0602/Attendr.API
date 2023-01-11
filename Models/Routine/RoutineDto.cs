namespace Attendr.API.Models.Routine
{
    public class RoutineDto
    {
        public Guid Id { get; set; }
        public string WeekDay { get; set; } = string.Empty;
        public Guid SemesterId { get; set; }
        public List<PeriodDto> Periods { get; set; } = new List<PeriodDto>(); // TODO PeriodDto
    }
}
