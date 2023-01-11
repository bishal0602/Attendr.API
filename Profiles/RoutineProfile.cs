using AutoMapper;

namespace Attendr.API.Profiles
{
    public class RoutineProfile : Profile
    {
        public RoutineProfile()
        {
            CreateMap<Entities.Routine, Models.Routine.RoutineDto>();
            CreateMap<Entities.Period, Models.Routine.PeriodDto>();
            CreateMap<Models.Routine.PeriodCreationDto, Entities.Period>();
        }
    }
}
