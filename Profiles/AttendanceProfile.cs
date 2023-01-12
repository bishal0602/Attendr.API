using AutoMapper;

namespace Attendr.API.Profiles
{
    public class AttendanceProfile : Profile
    {
        public AttendanceProfile()
        {
            CreateMap<Entities.Attendance, Models.Attendance.AttendanceDto>();
            CreateMap<Entities.AttendanceReport, Models.Attendance.AttendanceReportDto>();
            CreateMap<Models.Attendance.AttendanceUpdateDto, Entities.Attendance>();
            CreateMap<Models.Attendance.AttendanceReportUpdateDto, Entities.AttendanceReport>();
        }
    }
}
