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

        //[Authorize("cr")]

        [HttpGet("attendance/teachers/{teacherId}/today")]
        public async Task<ActionResult<AttendanceDto>> GetOrCreateTodaysAttendance([FromRoute] Guid teacherId)
        {
            // check teacher exists

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

        [HttpPut("attendance/teachers/{teacherId}/today")]
        public async Task<IActionResult> UpdateRoutine(AttendanceUpdateDto attendance)
        {
            //var attendanceFromDb = await _attendanceRepository.GetAttendanceByIdAsync(attendance.Id);
            //if (attendanceFromDb is null)
            //    return NotFound(new ErrorDetails(StatusCodes.Status404NotFound, $"Attendance with id {attendance.Id} not found!"));
            bool attendanceExists = await _attendanceRepository.ExistsAttendanceAsync(attendance.Id);
            if (attendanceExists == false)
            {
                return NotFound(new ErrorDetails(StatusCodes.Status404NotFound, $"Attendance with id {attendance.Id} not found!"));
            }

            var attendanceToUpdate = _mapper.Map<Entities.Attendance>(attendance);
            await _attendanceRepository.UpdateAttendanceAsync(attendanceToUpdate);
            await _attendanceRepository.SaveAsync();

            //var attendanceReports = await _attendanceRepository.GetAttendanceReportsByAttendanceIdAsync(attendance.Id);
            //_mapper.Map(attendance.AttendanceReports, attendanceReports);
            //await _attendanceRepository.SaveAsync();
            //foreach (var attendancerep in attendance.AttendanceReports)
            //{
            //    var atrp = attendanceReports.FirstOrDefault(ar => ar.Id == attendancerep.Id);
            //    if (atrp != null)
            //    {
            //        atrp.isPresent = attendancerep.IsPresent;
            //    }
            //}
            //await _attendanceRepository.SaveAsync();

            var attendanceReportsTest = await _attendanceRepository.GetAttendanceReportsByAttendanceIdAsync(attendance.Id);

            return Ok(attendanceReportsTest);

            //return NoContent();
        }

        //[Authorize("cr")]

        [HttpGet("attendance/teachers/{teacherId}")]
        public async Task<ActionResult<TeacherAttendanceReportDto>> GetTeachersAttendance([FromRoute] Guid teacherId)
        {
            var teacher = await _teacherRepository.GetTeacherByIdAsync(teacherId);
            if (teacher is null)
                return NotFound("Teacher not found!");

            var attendanceReports = await _attendanceRepository.GetTeachersAttendanceReport(teacherId);
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

    }
}
