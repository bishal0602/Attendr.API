using AutoMapper;

namespace Attendr.API.Profiles
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            CreateMap<Entities.Teacher, Models.Teacher.TeacherDto>();
            CreateMap<Models.Teacher.TeacherForCreationDto, Entities.Teacher>();
        }
    }
}
