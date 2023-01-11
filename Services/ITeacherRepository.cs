using Attendr.API.Entities;

namespace Attendr.API.Services
{
    public interface ITeacherRepository : IRepository
    {
        Task CreateTeacherAsync(Teacher teacher);
        Task<Teacher?> GetTeacherByIdAsync(Guid teacherId);

    }
}