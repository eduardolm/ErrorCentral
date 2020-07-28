using ErrorCentral.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ErrorCentral.Infra.Context
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Environment> Environment { get; set; }
        public DbSet<Layer> Layer { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<Level> Level { get; set; }
        public DbSet<Status> Status { get; set; }
        
           
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Server=127.0.0.1,1433;Database=ErrorCentral;User Id=SA;Password=<password>");
            }
        }
    }
}