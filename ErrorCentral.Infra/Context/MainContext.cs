using ErrorCentral.Domain.Mappings;
using ErrorCentral.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ErrorCentral.Infra.Context
{
    public class MainContext : DbContext
    {
        private IConfiguration Configuration { get; }
        public MainContext(DbContextOptions options) : base(options)
        {
        }
        
        public MainContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        public DbSet<User> User { get; set; }
        public DbSet<Environment> Environment { get; set; }
        public DbSet<Layer> Layer { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<Level> Level { get; set; }
        public DbSet<Status> Status { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMapConfig());
            
            base.OnModelCreating(modelBuilder);
        }
        
           
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }
        }
    }
}