using Newtonsoft.Json;

namespace Attendr.API.Models.Routine
{
    public class PeriodCreationDto
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public Guid TeacherId { get; set; }
    }
}
