using Attendr.API.Entities;

namespace Attendr.API.Services
{
    public interface IClassRepository
    {
        Task AddClassWithStudentsAsync(Class classToAdd);
        Task<List<Class>> GetClassesAsync();
        Task<Class?> GetClassByIdAsync(Guid classId);
        Task<Class?> GetClassByYearDepartGroupAsync(string classYear, string classDepartment, string classGroup);
        Task<bool> ExistsClassAsync(string classYear, string classDepartment, string classGroup);
        Task<bool> SaveAsync();
    }
}