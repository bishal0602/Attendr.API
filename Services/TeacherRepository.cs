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
        public async Task<Teacher?> GetTeacherByIdAsync(Guid teacherId)
        {
            return await _context.Teachers.FirstOrDefaultAsync(t => t.Id == teacherId);
        }


        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
