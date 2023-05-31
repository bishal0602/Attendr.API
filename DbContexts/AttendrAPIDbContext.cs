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
        public DbSet<Routine> Routines { get; set; } = null!;
        public DbSet<Period> Periods { get; set; } = null!;
        public DbSet<Semester> Semesters { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public AttendrAPIDbContext(DbContextOptions<AttendrAPIDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttendanceReport>()
                        .HasOne(ar => ar.Student)
                        .WithMany(s => s.AttendanceReports)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Attendance>()
                        .HasOne(a => a.Teacher)
                        .WithMany(t => t.Attendances)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Period>()
                        .HasOne(a => a.Teacher)
                        .WithMany(p => p.Periods)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Period>()
                        .HasOne(p => p.Routine)
                        .WithMany(r => r.Periods)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AttendanceReport>()
                        .HasOne(a => a.Attendance)
                        .WithMany(a => a.AttendanceReports)
                        .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
