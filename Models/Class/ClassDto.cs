using Attendr.API.Entities;
using Attendr.API.Models.Student;

namespace Attendr.API.Models.Class
{
    /// <summary>
    /// A class with id, year, department, group and optionally list of students and semester routines and teachers
    /// </summary>
    public class ClassDto
    {
        /// <summary>
        /// The id of the class
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// The admission year of the class
        /// </summary>
        public string Year { get; set; } = null!;
        /// <summary>
        /// The department of the class
        /// </summary>
        public string Department { get; set; } = null!;
        /// <summary>
        /// The group of class
        /// </summary>
        public string Group { get; set; } = null!;
        /// <summary>
        /// List of students in class
        /// </summary>
        public List<StudentDto> Students { get; set; } = new List<StudentDto>();
        /// <summary>
        /// List of semesters, further contains list of routines and teachers
        /// </summary>
        public List<SemesterDto> Semesters { get; set; } = new List<SemesterDto>();
    }
}
