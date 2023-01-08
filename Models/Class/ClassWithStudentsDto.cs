using Attendr.API.Models.Student;

namespace Attendr.API.Models.Class
{
    public class ClassWithStudentsDto : ClassDtoBase
    {
        public List<StudentDto> Students { get; set; } = new List<StudentDto>();
    }
}
