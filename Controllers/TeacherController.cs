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
        private readonly IIdentityHelper _identityHelper;
        private readonly IMapper _mapper;

        public TeacherController(ITeacherRepository teacherRepository, IIdentityHelper identityHelper, IMapper mapper)
        {
            _teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
            _identityHelper = identityHelper ?? throw new ArgumentNullException(nameof(identityHelper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
            teacherToAdd.SemesterId = await _identityHelper.GetSemesterIdUsingUserIdentityAsync(User, teacher.SemesterName);
            await _teacherRepository.CreateTeacherAsync(teacherToAdd);
            await _teacherRepository.SaveAsync();

            var teacherToReturn = _mapper.Map<TeacherDto>(teacherToAdd);

            return CreatedAtRoute("GetTeacherById", new
            {
                teacherId = teacherToReturn.Id
            },
                new { result = "Teacher successfully created!", details = "Check Loaction header to navigate!" }); // TODO : Standardize creation response
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherDto>>> GetSemesterTeachers([FromQuery] string? semester = null)
        {
            if (string.IsNullOrWhiteSpace(semester))
            {
                throw new ArgumentException($"Query parameter '{nameof(semester)}' cannot be null or whitespace.", nameof(semester));
            }

            // TODO  check for semester validity

            Guid classId = (await _identityHelper.GetClassUsingIdentityAsync(User))!.Id;

            var teachers = await _teacherRepository.GetSemesterTeachersAsync(classId, semester);
            IEnumerable<TeacherDto> teachersToReturn = _mapper.Map<List<TeacherDto>>(teachers);

            return Ok(teachersToReturn);
        }
    }
}
