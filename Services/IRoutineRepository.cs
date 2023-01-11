using Attendr.API.Entities;

namespace Attendr.API.Services
{
    public interface IRoutineRepository : IRepository
    {
        Task AddPeriodToRoutineAsync(Guid semesterId, string weekDay, Period period);
        Task<Routine?> GetRoutineByIdAsync(Guid routineId);
        Task<List<Routine>> GetRoutineForSemesterAsync(Guid semesterId);
    }
}