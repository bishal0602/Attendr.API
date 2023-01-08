using Attendr.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Attendr.API.DbContexts
{
    public class AttendrAPIDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Class> Classes { get; set; } = null!;

        public AttendrAPIDbContext(DbContextOptions<AttendrAPIDbContext> options) : base(options)
        {

        }
    }
}
