using Attendr.API.DataStore;
using Attendr.API.Entities;
using AutoMapper;

namespace Attendr.API.Helpers
{
    public class ClassStudentHelper : IClassStudentHelper
    {
        private readonly IMapper _mapper;
        private static readonly string alphabetes = "abcdefghijklmnopqrstuvwxyz";
        public ClassStudentHelper(IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public List<Student> GetAllStudentsBelongingToClass(string classYear, string classDepartment, string classGroup)
        {
            var studentsFromStore = StudentDataStore.StudentsList;
            var studentsInClass = studentsFromStore.Where(s => CheckWhetherStudentBelongsToClass(s.Email, classYear, classDepartment, classGroup));
            return _mapper.Map<List<Student>>(studentsInClass);
        }

        public bool CheckWhetherStudentBelongsToClass(string studentEmail, string classYear, string classDepartment, string classGroup)
        {
            (string studentYear, string studentDepartment, string studentGroup) = GetStudentsClassDetailsFromEmail(studentEmail);
            return (
                studentYear == classYear &&
                studentDepartment.ToLower() == classDepartment.ToLower() &&
                string.Compare(studentGroup, classGroup.ToLower()) == 0
                );
        }

        public (string studentYear, string studentDepartment, string studentGroup) GetStudentsClassDetailsFromEmail(string studentEmail)
        {
            var studentYear = studentEmail.Substring(0, 3);
            var studentDepartment = studentEmail.Substring(3, 3);
            var studentRollNumber = studentEmail.Substring(6, 3);
            int studentGroupAsNumber = Int32.Parse(studentRollNumber) / 24;
            if (studentGroupAsNumber > 24 * 26)
            {
                throw new ArgumentOutOfRangeException("Student roll number could not be asssigned to group");
            }
            var studentGroup = alphabetes[studentGroupAsNumber].ToString();
            return (studentYear, studentDepartment, studentGroup);
        }


    }
}
