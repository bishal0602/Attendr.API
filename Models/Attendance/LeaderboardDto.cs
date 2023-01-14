namespace Attendr.API.Models.Attendance
{
    public class LeaderboardDto
    {
        public int TotalClasses { get; set; }
        public List<StudentAttendanceReportDto> Leaderboard { get; set; } = new List<StudentAttendanceReportDto>();
    }
}
