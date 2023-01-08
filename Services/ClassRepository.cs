using Attendr.API.DbContexts;
using Attendr.API.Entities;
using Attendr.API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Attendr.API.Services
{
    public class ClassRepository : IClassRepository
    {
        private readonly AttendrAPIDbContext _context;
        private readonly IClassStudentHelper _classStudentHelper;

        public ClassRepository(AttendrAPIDbContext context, IClassStudentHelper classStudentHelper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _classStudentHelper = classStudentHelper ?? throw new ArgumentNullException(nameof(classStudentHelper));
        }

        public async Task AddClassWithStudentsAsync(Class classToAdd)
        {
            List<Student> studentsInClass = _classStudentHelper.GetAllStudentsBelongingToClass(classToAdd.Year, classToAdd.Department, classToAdd.Group);
            classToAdd.Students.AddRange(studentsInClass);
            await _context.Classes.AddAsync(classToAdd);
        }

        public async Task<List<Class>> GetClassesAsync()
        {
            return await _context.Classes.ToListAsync();
        }

        public async Task<Class?> GetClassByIdAsync(Guid classId)
        {
            return await _context.Classes.Include(c => c.Students).FirstOrDefaultAsync(c => c.Id == classId);
        }
        public async Task<Class?> GetClassByYearDepartGroupAsync(string classYear, string classDepartment, string classGroup)
        {
            return await _context.Classes.Include(c => c.Students).FirstOrDefaultAsync(c => c.Year == classYear && c.Department == classDepartment && c.Group == classGroup);
        }

        public async Task<bool> ExistsClassAsync(string classYear, string classDepartment, string classGroup)
        {
            return await _context.Classes.AnyAsync(c => c.Year == classYear && c.Department == classDepartment && c.Group == classGroup);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
