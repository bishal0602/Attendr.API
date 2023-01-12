using Attendr.API.Entities;

namespace Attendr.API.Services
{
    public interface IAttendanceRepository : IRepository
    {
        Task<Attendance> CreateAttendanceAsync(Guid teacherId, Guid classId, DateTime date);
        Task<Attendance?> GetAttendanceAsync(Guid teacherId, DateTime date);
        Task<Attendance?> GetAttendanceByIdAsync(Guid id);
    }
}