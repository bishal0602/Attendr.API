using AutoMapper;

namespace Attendr.API.Profiles
{
    public class ClassProfile : Profile
    {
        public ClassProfile()
        {
            CreateMap<Models.Class.ClassCreationDto, Entities.Class>();
            CreateMap<Entities.Class, Models.Class.ClassDto>();
            CreateMap<Entities.Class, Models.Class.ClassWithoutStudentsDto>();
            CreateMap<Entities.Semester, Models.Class.SemesterDto>().ReverseMap();
        }
    }
}
