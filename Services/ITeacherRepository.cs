using Attendr.API.Entities;

namespace Attendr.API.Services
{
    public interface ITeacherRepository : IRepository
    {
        Task CreateTeacherAsync(Teacher teacher);
        Task<int> GetNumberOfAttendancesTakenAsync(Guid teacherId);
        Task<IEnumerable<Teacher>> GetSemesterTeachersAsync(Guid classId, string semester);
        Task<Teacher?> GetTeacherByIdAsync(Guid teacherId, bool includeSemester = false);

    }
}