using Attendr.API.Entities;

namespace Attendr.API.Services
{
    public interface IRepository
    {
        Task<bool> SaveAsync();
    }
}
