using Attendr.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Attendr.API.DbContexts
{
    public class AttendrAPIDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Class> Classes { get; set; } = null!;
        public DbSet<Attendance> Attendances { get; set; } = null!;
        public DbSet<AttendanceReport> AttendanceReports { get; set; } = null!;
        public DbSet<Routine> DayRoutines { get; set; } = null!;
        public DbSet<Period> Periods { get; set; } = null!;
        public DbSet<Semester> Semesters { get; set; } = null!;
        public DbSet<TeacherSubject> TeacherSubjects { get; set; } = null!;
        public AttendrAPIDbContext(DbContextOptions<AttendrAPIDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttendanceReport>().HasOne(a => a.Student).WithMany(s => s.AttendanceReports).OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
