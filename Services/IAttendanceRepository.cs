using Attendr.API.Entities;
using Attendr.API.Models;

namespace Attendr.API.Services
{
    public interface IAttendanceRepository : IRepository
    {
        Task<Attendance> CreateAttendanceAsync(Guid teacherId, Guid classId, DateTime date);
        Task<bool> ExistsAttendanceAsync(Guid id);
        Task<int> GetTotalAttendanceForClass(Guid classId);
        Task<Attendance?> GetAttendanceAsync(Guid teacherId, DateTime date);
        Task<Attendance?> GetAttendanceByIdAsync(Guid id);
        Task<IEnumerable<StudentAttendanceReport>> GetTeachersAttendanceReportAsync(Guid teacherId);
        Task UpdateAttendanceAsync(Attendance attendance);
        Task<IEnumerable<AttendanceReport>> GetAttendanceReportsByAttendanceIdAsync(Guid attendanceId);
        Task<IEnumerable<StudentAttendanceReport>> GetOrderedClassAttendanceReportAsync(Guid classId);
        Task DeleteAttendanceByIdAsync(Guid attendanceId);
    }
}