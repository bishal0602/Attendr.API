namespace Attendr.API.Models.Student
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; } = string.Empty;
        public Guid ClassId { get; set; }
    }
}
