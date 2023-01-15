using Attendr.API.Helpers;
using Attendr.API.Models;
using Attendr.API.Models.Attendance;
using Attendr.API.Models.Teacher;
using Attendr.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Attendr.API.Controllers
{
    /// <response code="406">Invalid Accept header</response>
    /// <response code="415">Invalid Content-Type header</response>
    /// <response code="500">Internal Server Error</response>
    [Route("api/attendances")]
    [ApiController]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IIdentityHelper _identityHelper;
        private readonly IMapper _mapper;

        public AttendanceController(IAttendanceRepository attendanceRepository, ITeacherRepository teacherRepository, IIdentityHelper identityHelper, IMapper mapper)
        {
            _attendanceRepository = attendanceRepository ?? throw new ArgumentNullException(nameof(attendanceRepository));
            _teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
            _identityHelper = identityHelper ?? throw new ArgumentNullException(nameof(identityHelper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// [CR,ADMIN] Returns current date's attendance for the selected teacher
        /// </summary>
        /// <param name="teacherId">The id of the teacher</param>
        /// <returns></returns>
        [Authorize(Roles = "cr,admin")]
        [HttpGet("attendance/teachers/{teacherId}/today")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AttendanceDto))]
        public async Task<ActionResult<AttendanceDto>> GetOrCreateTodaysAttendance([FromRoute] Guid teacherId)
        {
            // TODO check teacher exists

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


        /// <summary>
        /// [CR,ADMIN] Updates teachers current date's routine
        /// </summary>
        /// <param name="teacherId">The id of the teacher</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request 
        /// 
        /// PUT /api/attendances/attendance/teachers/cf852468-91a0-4600-32ec-08daf577f48c/today
        /// 
        ///     {
        ///     "id": "4277f2e9-6fda-4e61-cf36-08daf6e654b7",
        ///     "date": "2023-01-15T00:00:00
        ///     "classId": "eae1d12a-ce9e-4f26-1a5d-08daf576c683",
        ///     "teacherId": "cf852468-91a0-4600-32ec-08daf577f48c",
        ///     "attendanceReports": [
        ///         {
        ///             "id": "2d33a85c-c194-4c36-fd82-08daf6e654bc",
        ///              "student": {
        ///                "id": "253d5ade-2a25-4535-720e-08daf576c697",
        ///                "name": "Julius Caesar",
        ///                "email": "078bct001.julius@pcampus.edu.np",
        ///                "phone": "9812345678",
        ///                "classId": "eae1d12a-ce9e-4f26-1a5d-08daf576c683"
        ///              },
        ///             "studentId": "9b0bdd2c-5b9f-43f1-720d-08daf576c697",
        ///             "isPresent": true,
        ///             "teacherId": "cf852468-91a0-4600-32ec-08daf577f48c",
        ///             "attendanceId": "4277f2e9-6fda-4e61-cf36-08daf6e654b7"
        ///         },
        ///         ]
        ///     }
        /// </remarks>
        [Authorize(Roles = "cr,admin")]
        [HttpPut("attendance/teachers/{teacherId}/today")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> UpdateRoutine(AttendanceUpdateDto attendance)
        {
            if (attendance.Date != DateTime.Today)
            {
                return BadRequest(new ErrorDetails(StatusCodes.Status400BadRequest, "Attendance could not be updated", "Attempt to update attendance from other day"));
            }
            bool attendanceExists = await _attendanceRepository.ExistsAttendanceAsync(attendance.Id);
            if (attendanceExists == false)
            {
                return NotFound(new ErrorDetails(StatusCodes.Status404NotFound, $"Attendance with id {attendance.Id} not found!"));
            }

            var attendanceToUpdate = _mapper.Map<Entities.Attendance>(attendance);
            await _attendanceRepository.UpdateAttendanceAsync(attendanceToUpdate);
            await _attendanceRepository.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// Returns teachers attendance report
        /// </summary>
        /// <param name="teacherId">The id of the teacher</param>
        /// <returns></returns>
        [HttpGet("attendance/teachers/{teacherId}/report")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeacherAttendanceReportDto))]
        public async Task<ActionResult<TeacherAttendanceReportDto>> GetTeachersAttendanceReport([FromRoute] Guid teacherId)
        {
            var teacher = await _teacherRepository.GetTeacherByIdAsync(teacherId);
            if (teacher is null)
                return NotFound("Teacher not found!");

            var attendanceReports = await _attendanceRepository.GetTeachersAttendanceReportAsync(teacherId);
            var attendanceReportsToReturn = _mapper.Map<List<StudentAttendanceReportDto>>(attendanceReports);

            TeacherAttendanceReportDto teacherAttendanceReportDto
                = new TeacherAttendanceReportDto()
                {
                    Teacher = _mapper.Map<TeacherDto>(teacher),
                    AttendanceReports = attendanceReportsToReturn,
                    TotalClass = await _teacherRepository.GetNumberOfAttendancesTakenAsync(teacherId),
                };

            return Ok(teacherAttendanceReportDto);
        }

        /// <summary>
        /// Returns class attendance leaderboard
        /// </summary>
        /// <returns></returns>
        [HttpGet("leaderboard")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LeaderboardDto))]
        public async Task<ActionResult<LeaderboardDto>> GetAttendanceLeaderboard()
        {

            Guid classId = (await _identityHelper.GetClassUsingIdentityAsync(User))!.Id;

            var orderedAttendanceReports = await _attendanceRepository.GetOrderedClassAttendanceReportAsync(classId);
            List<StudentAttendanceReportDto> orderedAttendanceReportsToReturn = _mapper.Map<List<StudentAttendanceReportDto>>(orderedAttendanceReports);

            LeaderboardDto leaderboard = new LeaderboardDto()
            {
                Leaderboard = orderedAttendanceReportsToReturn,
                TotalClasses = await _attendanceRepository.GetTotalAttendanceForClass(classId)
            };

            return Ok(leaderboard);

        }

        /// <summary>
        /// [CR, ADMIN] Delete attendance
        /// </summary>
        /// <param name="attendanceId"></param>
        /// <returns></returns>
        [Authorize(Roles = "cr,admin")]
        [HttpDelete("{attendanceId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAttendance(Guid attendanceId)
        {
            bool attendanceExists = await _attendanceRepository.ExistsAttendanceAsync(attendanceId);
            if (!attendanceExists)
            {
                return NotFound(new ErrorDetails(StatusCodes.Status404NotFound, $"Attendance not found!"));
            }

            await _attendanceRepository.DeleteAttendanceByIdAsync(attendanceId);
            await _attendanceRepository.SaveAsync();

            return NoContent();
        }

    }
}
