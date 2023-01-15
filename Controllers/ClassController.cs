using Attendr.API.Helpers;
using Attendr.API.Models;
using Attendr.API.Models.Class;
using Attendr.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Attendr.API.Controllers
{
    /// <response code="406">Invalid Accept header</response>
    /// <response code="415">Invalid Content-Type header</response>
    /// <response code="500">Internal Server Error</response>
    [Route("api/classes")]
    [ApiController]
    [Authorize]
    public class ClassController : ControllerBase
    {
        private readonly IClassRepository _classRepository;
        private readonly IIdentityHelper _identityHelper;
        private readonly IMapper _mapper;

        public ClassController(IClassRepository classRepository,

                               IIdentityHelper identityHelper,
                               IMapper mapper)
        {
            _classRepository = classRepository ?? throw new ArgumentNullException(nameof(classRepository));
            _identityHelper = identityHelper ?? throw new ArgumentNullException(nameof(identityHelper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// [ADMIN] Returns list of all classes
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetClasses()
        {
            var classesFromDb = await _classRepository.GetClassesAsync();
            var classToReturn = _mapper.Map<List<ClassDto>>(classesFromDb);
            return Ok(classToReturn);
        }

        /// <summary>
        /// [ADMIN] Returns specific class by id
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="includeStudents"></param>
        /// <param name="includeRoutine"></param>
        /// <param name="includeTeachers"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpGet("{classId}", Name = "GetClassById")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClassDto))]
        public async Task<IActionResult> GetClassById([FromRoute] Guid classId, [FromQuery] bool includeStudents = false, bool includeRoutine = false, bool includeTeachers = false)
        {
            var classFromDb = await _classRepository.GetClassByIdAsync(classId, includeStudents, includeRoutine, includeTeachers);

            if (classFromDb is null)
                return NotFound(new ErrorDetails(StatusCodes.Status404NotFound, $"Class with id {classId} does not exist!"));

            return Ok(_mapper.Map<ClassDto>(classFromDb));
        }

        /// <summary>
        /// [ADMIN] Adds class to the database, along with the list of students if available from local store
        /// </summary>
        /// <param name="Class">Class to add</param>
        /// <returns>201 Created with uri of the created class in Location header</returns>
        /// <response code="422">Model Validation Error</response>
        /// <remarks>
        /// Sample request to add 078-BCT-B class:  
        /// 
        ///     POST /api/classes
        ///     {
        ///         "year":"078",
        ///         "department":"bct",
        ///         "group":"b"
        ///     }
        /// </remarks>
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatedSuccessResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]

        public async Task<IActionResult> AddClass([FromBody] ClassCreationDto Class)
        {
            var classAlreadyExists = await _classRepository.ExistsClassAsync(Class.Year, Class.Department, Class.Group);
            if (classAlreadyExists)
            {
                return BadRequest(new ErrorDetails(StatusCodes.Status400BadRequest, "Class Already Exists!"));
            }
            var classToAdd = _mapper.Map<Entities.Class>(Class);

            await _classRepository.AddClassWithStudentsAsync(classToAdd);
            await _classRepository.SaveAsync();

            return CreatedAtRoute("GetClassById", new
            {
                classId = classToAdd.Id,
            }, new CreatedSuccessResponse(nameof(Class)));
        }

        /// <summary>
        /// Returns logged in users class
        /// </summary>
        /// <param name="includeStudents">Whether to include students</param>
        /// <param name="includeRoutines">Whether to include routines</param>
        /// <param name="includeTeachers">Whether to include teachers</param>
        /// <returns>An ActionResult of type Class</returns>
        /// <response code="200">Returns the logged in user's class</response>
        [HttpGet("myclass")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClassDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<ClassDto>> GetLoggedInUsersClass([FromQuery] bool includeStudents = false, [FromQuery] bool includeRoutines = false, [FromQuery] bool includeTeachers = false)
        {

            var userClass = await _identityHelper.GetClassUsingIdentityAsync(User, includeStudents, includeRoutines, includeTeachers);
            if (userClass is null)
                return NotFound(new ErrorDetails(StatusCodes.Status404NotFound, "User is not assigned to class"));

            return Ok(_mapper.Map<ClassDto>(userClass));
        }

        /// <summary>
        /// Returns list of semesters
        /// </summary>
        /// <returns></returns>
        [HttpGet("myclass/semesters")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SemesterDto>))]
        public async Task<ActionResult<IEnumerable<SemesterDto>>> GetSemesters()
        {
            var userClass = await _identityHelper.GetClassUsingIdentityAsync(User);
            if (userClass is null)
            {
                return BadRequest(new ErrorDetails(StatusCodes.Status400BadRequest, "User is not assigned to class"));
            }
            var semesters = await _classRepository.GetSemestersAsync(userClass.Id);
            return Ok(_mapper.Map<List<SemesterDto>>(semesters));
        }

    }
}
