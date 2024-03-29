﻿namespace Attendr.API.Entities
{
    public class Period
    {
        public Guid Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public Routine Routine { get; set; }
        public Guid RoutineId { get; set; }
        public Teacher Teacher { get; set; }
        public Guid TeacherId { get; set; }
    }
}
