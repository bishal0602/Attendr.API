using Attendr.API.Helpers;
using Attendr.API.Models;
using Attendr.API.Models.Teacher;
using Attendr.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace Attendr.API.Controllers
{
    /// <response code="406">Invalid Accept header</response>
    /// <response code="415">Invalid Content-Type header</response>
    /// <response code="500">Internal Server Error</response>
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

        /// <summary>
        /// Returns teacher with specified id
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        [HttpGet("{teacherId}", Name = "GetTeacherById")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeacherDto))]
        public async Task<ActionResult<TeacherDto>> GetTeacher([FromRoute] Guid teacherId)
        {
            var teacher = await _teacherRepository.GetTeacherByIdAsync(teacherId);
            if (teacher is null)
                return NotFound(new ErrorDetails(StatusCodes.Status404NotFound, "Teacher Not Found", $"Teacher with id {teacherId} was not found"));

            var teacherToReturn = _mapper.Map<TeacherDto>(teacher);
            return Ok(teacherToReturn);
        }

        /// <summary>
        /// [CR,ADMIN] Adds teacher to specified semester
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        /// POST to /api/teachers
        /// 
        ///     {
        ///       "name": "Ganesh Shrestha",
        ///       "shortname": "GS",
        ///       "subject": "Physics",
        ///       "semester": "first"
        ///     }
        /// </remarks>
        [Authorize(Roles = "cr,admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatedSuccessResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
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
                new CreatedSuccessResponse(nameof(teacher)));
        }

        /// <summary>
        /// Returns specified semester's teachers
        /// </summary>
        /// <param name="semester">Name of the semester</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherDto>>> GetSemesterTeachers([FromQuery][BindRequired] string semester)
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
