namespace Attendr.API.Models.Class
{
    public abstract class ClassDtoBase
    {
        public Guid Id { get; set; }
        public string Year { get; set; }
        public string Department { get; set; }
        public string Group { get; set; }
    }
}