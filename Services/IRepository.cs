namespace Attendr.API.Services
{
    public interface IRepository
    {
        Task<bool> SaveAsync();
    }
}
