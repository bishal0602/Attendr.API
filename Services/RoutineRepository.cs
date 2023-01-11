using Attendr.API.DbContexts;
using Attendr.API.Entities;
using Attendr.API.Models.Routine;
using Microsoft.EntityFrameworkCore;

namespace Attendr.API.Services
{
    public class RoutineRepository : IRoutineRepository
    {
        private readonly AttendrAPIDbContext _context;

        public RoutineRepository(AttendrAPIDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddPeriodToRoutineAsync(Guid semesterId, string weekDay, Period period)
        {
            var routine = await _context.Routines.FirstOrDefaultAsync(r => r.SemesterId == semesterId && r.WeekDay == weekDay.Trim().ToLower());
            if (routine is null)
            {
                throw new ArgumentException($"Routine with semester Id {semesterId} and week day {weekDay} not found");
            }
            routine.Periods.Add(period);
        }

        public async Task<Routine?> GetRoutineByIdAsync(Guid routineId)
        {
            return await _context.Routines.Include(r => r.Periods).ThenInclude(p => p.Teacher).FirstOrDefaultAsync(r => r.Id == routineId);
        }

        public async Task<List<Routine>> GetRoutineForSemesterAsync(Guid semesterId)
        {
            var routines = await _context.Routines.Where(r => r.SemesterId == semesterId).Include(r => r.Periods).ThenInclude(p => p.Teacher).ToListAsync();
            return routines;
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
