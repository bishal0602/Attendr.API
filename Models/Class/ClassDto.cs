using Attendr.API.Entities;
using Attendr.API.Models.Student;

namespace Attendr.API.Models.Class
{
    public class ClassDto : ClassDtoBase
    {
        public Guid Id { get; set; }
        public List<StudentDto> Students { get; set; } = new List<StudentDto>();
        // TODO: List of Semester dto
    }
}
