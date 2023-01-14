using Attendr.API.DbContexts;
using Attendr.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Attendr.API.Services
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly AttendrAPIDbContext _context;

        public TeacherRepository(AttendrAPIDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task CreateTeacherAsync(Teacher teacher)
        {
            await _context.Teachers.AddAsync(teacher);
        }

        public async Task<int> GetNumberOfAttendancesTakenAsync(Guid teacherId)
        {
            return await _context.Attendances.CountAsync(a => a.TeacherId == teacherId);
        }

        public async Task<IEnumerable<Teacher>> GetSemesterTeachersAsync(Guid classId, string semester)
        {
            return await _context.Teachers.Where(t => t.Semester.ClassId == classId && t.Semester.Name == semester.Trim().ToLower()).ToListAsync();
        }

        public async Task<Teacher?> GetTeacherByIdAsync(Guid teacherId, bool includeSemester = false)
        {
            if (includeSemester)
                return await _context.Teachers.Include(t => t.Semester).FirstOrDefaultAsync(t => t.Id == teacherId);

            return await _context.Teachers.FirstOrDefaultAsync(t => t.Id == teacherId);
        }


        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
