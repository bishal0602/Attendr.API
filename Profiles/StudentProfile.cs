using Attendr.API.DataStore;
using AutoMapper;

namespace Attendr.API.Profiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<StudentDataStore.StudentDataStoreModel, Entities.Student>();
            CreateMap<Entities.Student, Models.Student.StudentDto>();
        }
    }
}
