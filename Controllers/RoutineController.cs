using Attendr.API.Helpers;
using Attendr.API.Models.Routine;
using Attendr.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Attendr.API.Controllers
{
    [Route("api/routines")]
    [ApiController]
    [Authorize]
    public class RoutineController : ControllerBase
    {
        private readonly IRoutineRepository _routineRepository;
        private readonly IIdentityHelper _identityHelper;
        private readonly IMapper _mapper;

        public RoutineController(IRoutineRepository routineRepository, IIdentityHelper identityHelper, IMapper mapper)
        {
            _routineRepository = routineRepository ?? throw new ArgumentNullException(nameof(routineRepository));
            _identityHelper = identityHelper ?? throw new ArgumentNullException(nameof(identityHelper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("semesters/{semester}")]
        public async Task<ActionResult<IEnumerable<RoutineDto>>> GetRoutine(string semester)
        {
            // TODO validate semester

            Guid semesterId = await _identityHelper.GetSemesterIdUsingUserIdentityAsync(User, semester);

            var routineFromDb = await _routineRepository.GetRoutineForSemesterAsync(semesterId);

            IEnumerable<RoutineDto> routineToReturn = _mapper.Map<List<RoutineDto>>(routineFromDb);

            return Ok(routineToReturn);
        }

        [HttpPost("semesters/{semester}/{weekDay}/periods")]
        //[Authorize("cr")]
        public async Task<IActionResult> AddPeriodToRoutine([FromRoute] string semester, [FromRoute] string weekDay, [FromBody] PeriodCreationDto period)
        {
            // TODO validate semester and weeekday

            Guid semesterId = await _identityHelper.GetSemesterIdUsingUserIdentityAsync(User, semester);

            Entities.Period periodToAdd = _mapper.Map<Entities.Period>(period);
            await _routineRepository.AddPeriodToRoutineAsync(semesterId, weekDay, periodToAdd);
            await _routineRepository.SaveAsync();

            return CreatedAtRoute("GetRoutine", new
            {
                routineId = periodToAdd.RoutineId
            }, "Period Added. Check Location Header to navigate to routine"); // TODO: Standaradize success response

        }

        [HttpGet("{routineId}", Name = "GetRoutine")]
        public async Task<ActionResult<RoutineDto>> GetRoutine(Guid routineId)
        {
            var routine = await _routineRepository.GetRoutineByIdAsync(routineId);

            RoutineDto routineToReturn = _mapper.Map<RoutineDto>(routine);

            return Ok(routineToReturn);
        }

    }
}
