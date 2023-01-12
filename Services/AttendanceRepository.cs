using Attendr.API.DbContexts;
using Attendr.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attendr.API.Services
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AttendrAPIDbContext _context;

        public AttendanceRepository(AttendrAPIDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Attendance?> GetAttendanceAsync(Guid teacherId, DateTime date)
        {
            return await _context.Attendances.Include(a => a.AttendanceReports).ThenInclude(ar => ar.Student).FirstOrDefaultAsync(a => a.TeacherId == teacherId && a.Date == date);
        }

        public async Task<Attendance> CreateAttendanceAsync(Guid teacherId, Guid classId, DateTime date)
        {
            Attendance attendance = new()
            {
                TeacherId = teacherId,
                ClassId = classId,
                Date = date
            };

            List<AttendanceReport> attendaceReports = new();

            var studentsInClass = await _context.Students.Where(s => s.ClassId == classId).ToListAsync();
            studentsInClass.ForEach(student =>
            {
                AttendanceReport attendanceReport = new()
                {
                    StudentId = student.Id,
                    isPresent = null,
                };
                attendaceReports.Add(attendanceReport);
            });

            attendaceReports.AddRange(attendaceReports);
            attendance.AttendanceReports.AddRange(attendaceReports);

            await _context.AddAsync(attendance);
            return attendance;
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<Attendance?> GetAttendanceByIdAsync(Guid id)
        {
            return await _context.Attendances.FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
