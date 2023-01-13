using Attendr.API.Entities;
using Attendr.API.Services.Models;

namespace Attendr.API.Services
{
    public interface IAttendanceRepository : IRepository
    {
        Task<Attendance> CreateAttendanceAsync(Guid teacherId, Guid classId, DateTime date);
        Task<bool> ExistsAttendanceAsync(Guid id);
        Task<Attendance?> GetAttendanceAsync(Guid teacherId, DateTime date);
        Task<Attendance?> GetAttendanceByIdAsync(Guid id);
        Task<IEnumerable<StudentAttendanceReport>> GetTeachersAttendanceReport(Guid teacherId);
        Task UpdateAttendanceAsync(Attendance attendance);
        Task<IEnumerable<AttendanceReport>> GetAttendanceReportsByAttendanceIdAsync(Guid attendanceId);
    }
}