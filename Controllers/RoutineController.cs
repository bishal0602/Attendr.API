using Attendr.API.Entities;
using Attendr.API.Helpers;
using Attendr.API.Models;
using Attendr.API.Models.Routine;
using Attendr.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendr.API.Controllers
{
    /// <response code="406">Invalid Accept header</response>
    /// <response code="415">Invalid Content-Type header</response>
    /// <response code="500">Internal Server Error</response>
    [Route("api/routines")]
    [ApiController]
    [Authorize]
    public class RoutineController : ControllerBase
    {
        private readonly IRoutineRepository _routineRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IIdentityHelper _identityHelper;
        private readonly IMapper _mapper;

        public RoutineController(IRoutineRepository routineRepository, ITeacherRepository teacherRepository, IIdentityHelper identityHelper, IMapper mapper)
        {
            _routineRepository = routineRepository ?? throw new ArgumentNullException(nameof(routineRepository));
            _teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
            _identityHelper = identityHelper ?? throw new ArgumentNullException(nameof(identityHelper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Returns semesters routine
        /// </summary>
        /// <param name="semester">Name of the semester</param>
        /// <returns></returns>
        [HttpGet("semesters/{semester}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RoutineDto>))]
        public async Task<ActionResult<IEnumerable<RoutineDto>>> GetRoutine(string semester)
        {
            // TODO validate semester

            Guid semesterId = await _identityHelper.GetSemesterIdUsingUserIdentityAsync(User, semester);

            var routineFromDb = await _routineRepository.GetRoutineForSemesterAsync(semesterId);

            IEnumerable<RoutineDto> routineToReturn = _mapper.Map<List<RoutineDto>>(routineFromDb);

            return Ok(routineToReturn);
        }

        /// <summary>
        /// [CR, ADMIN] Adds period to routine
        /// </summary>
        /// <param name="semester">Name of the semester</param>
        /// <param name="weekDay">Day of the week</param>
        /// <param name="period">The period to add</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request
        /// 
        /// POST to api/routines/semesters/second/monday/periods
        /// 
        ///     {
        ///         "startTime": "11:45am",
        ///         "endTime": "12:30pm",
        ///         "teacherId":"cf852468-91a0-4600-32ec-08daf577f48c"
        ///     }
        /// </remarks>
        [Authorize(Roles = "cr,admin")]
        [HttpPost("semesters/{semester}/{weekDay}/periods")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatedSuccessResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> AddPeriodToRoutine([FromRoute] string semester, [FromRoute] string weekDay, [FromBody] PeriodCreationDto period)
        {
            // TODO validate semester and weeekday

            Guid semesterId = await _identityHelper.GetSemesterIdUsingUserIdentityAsync(User, semester);

            bool includeSemester = true;
            var teacher = await _teacherRepository.GetTeacherByIdAsync(period.TeacherId, includeSemester);
            if (teacher is null)
                return NotFound(new ErrorDetails(StatusCodes.Status404NotFound, $"Teacher with id: {period.TeacherId} does not exist"));

            if (teacher.Semester.Id != semesterId)
            {
                return BadRequest(new ErrorDetails(StatusCodes.Status400BadRequest, $"Invalid assignment of {teacher.Semester.Name} semester teacher to {semester} semester period"));
            }

            Entities.Period periodToAdd = _mapper.Map<Entities.Period>(period);
            await _routineRepository.AddPeriodToRoutineAsync(semesterId, weekDay, periodToAdd);
            await _routineRepository.SaveAsync();

            return CreatedAtRoute("GetRoutine", new
            {
                routineId = periodToAdd.RoutineId
            }, new CreatedSuccessResponse(nameof(period)));

        }

        /// <summary>
        /// Returns routine by id
        /// </summary>
        /// <param name="routineId">Id of the routine</param>
        /// <returns></returns>
        [HttpGet("{routineId}", Name = "GetRoutine")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoutineDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<RoutineDto>> GetRoutine(Guid routineId)
        {
            var routine = await _routineRepository.GetRoutineByIdAsync(routineId);
            if (routine is null)
                return NotFound(new ErrorDetails(StatusCodes.Status404NotFound, $"Routine not found"));

            RoutineDto routineToReturn = _mapper.Map<RoutineDto>(routine);

            return Ok(routineToReturn);
        }

    }
}
