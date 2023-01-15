using Attendr.API.Services;
using System.Security.Claims;

namespace Attendr.API.Helpers
{
    public class IdentityHelper : IIdentityHelper
    {
        private readonly IClassStudentHelper _classStudentHelper;
        private readonly IClassRepository _classRepository;

        public IdentityHelper(IClassStudentHelper classStudentHelper, IClassRepository classRepository)
        {
            _classStudentHelper = classStudentHelper ?? throw new ArgumentNullException(nameof(classStudentHelper));
            _classRepository = classRepository ?? throw new ArgumentNullException(nameof(classRepository));
        }
        public async Task<Guid> GetSemesterIdUsingUserIdentityAsync(ClaimsPrincipal User, string semester)
        {
            if (User is null)
            {
                throw new ArgumentNullException(nameof(User));
            }

            var userClaims = ((ClaimsIdentity)User.Identity!).Claims;
            var userEmail = userClaims.FirstOrDefault(c => c.Type == "email")?.Value;
            if (userEmail == null)
            {
                throw new Exception("No email provided in claims");
            }
            (string studentYear, string studentDepartment, string studentGroup) = _classStudentHelper.GetStudentsClassDetailsFromEmail(userEmail);

            return await _classRepository.GetSemesterIdAsync(studentYear, studentDepartment, studentGroup, semester);
        }

        public async Task<Entities.Class?> GetClassUsingIdentityAsync(ClaimsPrincipal User, bool includeStudents = false, bool includeRoutines = false, bool includeTeachers = false)
        {
            if (User is null)
            {
                throw new ArgumentNullException(nameof(User));
            }

            var userClaims = ((ClaimsIdentity)User.Identity!).Claims;
            var userEmail = userClaims.FirstOrDefault(c => c.Type == "email")?.Value;
            if (userEmail == null)
            {
                throw new Exception("No email provided in claims");
            }
            (string studentYear, string studentDepartment, string studentGroup) = _classStudentHelper.GetStudentsClassDetailsFromEmail(userEmail);

            var classFromDb = await _classRepository.GetClassByYearDepartGroupAsync(studentYear, studentDepartment, studentGroup, includeStudents, includeRoutines, includeTeachers);
            return classFromDb;
        }
    }
}
