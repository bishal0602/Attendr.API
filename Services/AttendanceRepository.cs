using Attendr.API.DbContexts;
using Attendr.API.Entities;
using Attendr.API.Models;
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
                    TeacherId = teacherId,
                    StudentId = student.Id,
                    IsPresent = null,
                };
                attendaceReports.Add(attendanceReport);
            });

            attendaceReports.AddRange(attendaceReports);
            attendance.AttendanceReports.AddRange(attendaceReports);

            await _context.AddAsync(attendance);
            return attendance;
        }

        public async Task<Attendance?> GetAttendanceByIdAsync(Guid id)
        {
            return await _context.Attendances.FirstOrDefaultAsync(a => a.Id == id);
        }


        public async Task<IEnumerable<StudentAttendanceReport>> GetTeachersAttendanceReportAsync(Guid teacherId)
        {
            Teacher? teacher = await _context.Teachers.Include(t => t.Semester).FirstOrDefaultAsync(t => t.Id == teacherId);
            if (teacher is null)
                throw new Exception($"Teacher with id {teacherId} was not found");

            Guid classId = teacher.Semester.ClassId;

            List<Student> students = await _context.Students.Where(s => s.ClassId == classId).Include(s => s.AttendanceReports).OrderBy(s => s.Email).ToListAsync();

            List<StudentAttendanceReport> studentAttendanceReports = new();
            foreach (Student student in students)
            {
                StudentAttendanceReport studentAttendanceReport = new();
                studentAttendanceReport.Student = student;
                foreach (AttendanceReport attendanceReport in student.AttendanceReports)
                {
                    if (attendanceReport.TeacherId == teacherId)
                    {
                        studentAttendanceReport.TotalClass += 1;

                        if (attendanceReport.IsPresent == true)
                        {
                            studentAttendanceReport.TotalClassAttended += 1;
                        }
                    }
                }
                studentAttendanceReports.Add(studentAttendanceReport);
            }
            return studentAttendanceReports;
        }

        public async Task<IEnumerable<AttendanceReport>> GetAttendanceReportsByAttendanceIdAsync(Guid attendanceId)
        {

            var attendanceReports = await _context.AttendanceReports.Include(ar => ar.Student).Where(a => a.AttendanceId == attendanceId).ToListAsync();
            return attendanceReports;
        }

        public async Task<bool> ExistsAttendanceAsync(Guid id)
        {
            return await _context.Attendances.AnyAsync(a => a.Id == id);
        }

        public async Task UpdateAttendanceAsync(Attendance attendance)
        {
            var attendanceReports = await GetAttendanceReportsByAttendanceIdAsync(attendance.Id);
            foreach (var attendanceReport in attendance.AttendanceReports)
            {
                var atrp = attendanceReports.FirstOrDefault(ar => ar.Id == attendanceReport.Id);
                if (atrp != null)
                {
                    atrp.IsPresent = attendanceReport.IsPresent;
                }
            }
        }

        public async Task<int> GetTotalAttendanceForClass(Guid classId)
        {
            var attendances = await _context.Attendances.Where(a => a.ClassId == classId).ToListAsync();
            return attendances.Count();
        }

        public async Task<IEnumerable<StudentAttendanceReport>> GetOrderedClassAttendanceReportAsync(Guid classId)
        {

            List<Student> studentsInClass = await _context.Students.Where(s => s.ClassId == classId).Include(s => s.AttendanceReports).OrderBy(s => s.Email).ToListAsync();

            List<StudentAttendanceReport> studentAttendanceReports = new();
            foreach (Student student in studentsInClass)
            {
                StudentAttendanceReport studentAttendanceReport = new();
                studentAttendanceReport.Student = student;
                foreach (AttendanceReport attendanceReport in student.AttendanceReports)
                {
                    studentAttendanceReport.TotalClass += 1;

                    if (attendanceReport.IsPresent == true)
                    {
                        studentAttendanceReport.TotalClassAttended += 1;
                    }
                }
                studentAttendanceReports.Add(studentAttendanceReport);
            }
            return studentAttendanceReports.OrderBy(ar => ar.TotalClassAttended).Reverse();

        }

        public async Task DeleteAttendanceByIdAsync(Guid attendanceId)
        {
            // TODO
            var attendance = await _context.Attendances.FirstOrDefaultAsync(a => a.Id == attendanceId);
            if (attendance == null)
            {
                throw new ArgumentException($"Attendance with id {attendanceId} not found!");
            }
            _context.Attendances.Remove(attendance);

        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}

