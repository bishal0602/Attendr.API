using Attendr.API.Entities;

namespace Attendr.API.Helpers
{
    public interface IClassStudentHelper
    {
        bool CheckWhetherStudentBelongsToClass(string studentEmail, string classYear, string classDepartment, string classGroup);
        List<Student> GetAllStudentsBelongingToClass(string classYear, string classDepartment, string classGroup);
        (string studentYear, string studentDepartment, string studentGroup) GetStudentsClassDetailsFromEmail(string studentEmail);
    }
}