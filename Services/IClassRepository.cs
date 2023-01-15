using Attendr.API.Entities;

namespace Attendr.API.Services
{
    public interface IClassRepository : IRepository
    {
        Task AddClassWithStudentsAsync(Class classToAdd);
        Task<List<Class>> GetClassesAsync();
        Task<Class?> GetClassByIdAsync(Guid classId, bool includeStudents = false, bool includeRoutine = false, bool includeTeachers = false);
        Task<Class?> GetClassByYearDepartGroupAsync(string classYear, string classDepartment, string classGroup, bool includeStudents = false, bool includeRoutine = false, bool includeTeachers = false);
        Task<bool> ExistsClassAsync(string classYear, string classDepartment, string classGroup);
        Task<List<Semester>> GetSemestersAsync(Guid classId);
        Task<Guid> GetSemesterIdAsync(string studentYear, string studentDepartment, string studentGroup, string semesterName);
    }
}