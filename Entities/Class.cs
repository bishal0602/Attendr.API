using System.ComponentModel.DataAnnotations;

namespace Attendr.API.Entities
{
    public class Class
    {
        public Guid Id { get; set; }
        public string Year { get; set; }
        public string Department { get; set; }
        public string Group { get; set; }
        public List<Student> Students { get; set; } = new List<Student>();
        //public Class(string year, string department, string group)
        //{
        //    Year = year;
        //    Department = department;
        //    Group = group;
        //}
    }
}
