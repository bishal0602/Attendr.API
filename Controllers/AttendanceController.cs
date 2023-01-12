using Attendr.API.Helpers;
using Attendr.API.Models;
using Attendr.API.Models.Attendance;
using Attendr.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Attendr.API.Controllers
{
    [Route("api/attendances")]
    [ApiController]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IIdentityHelper _identityHelper;
        private readonly IMapper _mapper;

        public AttendanceController(IAttendanceRepository attendanceRepository, IIdentityHelper identityHelper, IMapper mapper)
        {
            _attendanceRepository = attendanceRepository ?? throw new ArgumentNullException(nameof(attendanceRepository));
            _identityHelper = identityHelper ?? throw new ArgumentNullException(nameof(identityHelper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        //[Authorize("cr")]

        [HttpGet("attendance/teachers/{teacherId}/today")]
        public async Task<ActionResult<AttendanceDto>> GetOrCreateTodaysAttendance([FromRoute] Guid teacherId)
        {
            DateTime date = DateTime.Today;

            var attendance = await _attendanceRepository.GetAttendanceAsync(teacherId, date);
            if (attendance is null)
            {
                Guid classId = (await _identityHelper.GetClassUsingIdentityAsync(User))!.Id;
                await _attendanceRepository.CreateAttendanceAsync(teacherId, classId, date)!;
                await _attendanceRepository.SaveAsync();

                attendance = await _attendanceRepository.GetAttendanceAsync(teacherId, date);
            }

            var attendanceToReturn = _mapper.Map<AttendanceDto>(attendance);
            return Ok(attendanceToReturn);

        }

        //[Authorize("cr")]

        [HttpPut("attendance/teachers/{teacherId}")]
        public async Task<IActionResult> UpdateRoutine(AttendanceUpdateDto attendance)
        {
            var attendanceFromDb = await _attendanceRepository.GetAttendanceByIdAsync(attendance.Id);
            if (attendanceFromDb is null)
                return NotFound(new ErrorDetails(StatusCodes.Status404NotFound, $"Attendance with id {attendance.Id} not found!"));

            _mapper.Map<AttendanceUpdateDto, Entities.Attendance>(attendance, attendanceFromDb);
            await _attendanceRepository.SaveAsync();

            return NoContent();
        }
    }
}
