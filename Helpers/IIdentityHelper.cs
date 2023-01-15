using Attendr.API.Entities;
using System.Security.Claims;

namespace Attendr.API.Helpers
{
    public interface IIdentityHelper
    {
        Task<Class?> GetClassUsingIdentityAsync(ClaimsPrincipal User, bool includeStudents = false, bool includeRoutine = false, bool includeTeachers = false);
        Task<Guid> GetSemesterIdUsingUserIdentityAsync(ClaimsPrincipal User, string semester);
    }
}