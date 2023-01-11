using Attendr.API.Helpers;
using Attendr.API.Models;
using Attendr.API.Models.Teacher;
using Attendr.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Attendr.API.Controllers
{
    [Route("api/teachers")]
    [ApiController]
    [Authorize]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IClassRepository _classRepository;
        private readonly IClassStudentHelper _classStudentHelper;
        private readonly IMapper _mapper;

        public TeacherController(ITeacherRepository teacherRepository, IClassRepository classRepository, IClassStudentHelper classStudentHelper, IMapper mapper)
        {
            _teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
            _classRepository = classRepository ?? throw new ArgumentNullException(nameof(classRepository));
            _classStudentHelper = classStudentHelper ?? throw new ArgumentNullException(nameof(classStudentHelper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private async Task<Guid> GetSemesterIdUsingUserIdentityAsync(string semesterName)
        {
            var userClaims = ((ClaimsIdentity)User.Identity!).Claims;
            var userEmail = userClaims.FirstOrDefault(c => c.Type == "email")?.Value;
            if (userEmail == null)
            {
                throw new Exception("No email provided in claims");
            }
            (string studentYear, string studentDepartment, string studentGroup) = _classStudentHelper.GetStudentsClassDetailsFromEmail(userEmail);

            return await _classRepository.GetSemesterIdAsync(studentYear, studentDepartment, studentGroup, semesterName);
        }

        [HttpGet("{teacherId}", Name = "GetTeacherById")]
        public async Task<ActionResult<TeacherDto>> GetTeacher([FromRoute] Guid teacherId)
        {
            var teacher = await _teacherRepository.GetTeacherByIdAsync(teacherId);
            if (teacher is null)
                return NotFound(new ErrorDetails(StatusCodes.Status404NotFound, "Teacher Not Found", $"Teacher with id {teacherId} was not found"));

            var teacherToReturn = _mapper.Map<TeacherDto>(teacher);
            return Ok(teacherToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacher([FromBody] TeacherForCreationDto teacher)
        {
            var teacherToAdd = _mapper.Map<Entities.Teacher>(teacher);
            teacherToAdd.SemesterId = await GetSemesterIdUsingUserIdentityAsync(teacher.SemesterName);
            await _teacherRepository.CreateTeacherAsync(teacherToAdd);
            await _teacherRepository.SaveAsync();

            var teacherToReturn = _mapper.Map<TeacherDto>(teacherToAdd);

            return CreatedAtRoute("GetTeacherById", new
            {
                teacherId = teacherToReturn.Id
            },
                new { result = "Teacher successfully created!", details = "Check Loaction header to navigate!" }); // TODO : Standardize creation response
        }
    }
}
