/**
 * Clean Architecture Framework by Mohammad Hafijur Rahman
 * Implements Mediator, Factory, Unit of Work, and Repository patterns
 * Free to use - please keep this credit
 * https://github.com/mail4hafij/dotnet-clean-architecture-demo
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Core.DB.Builder;
using Core.DB.Model;

namespace Core.DB
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The following constructor is used by the migration script.
        /// The Core project does not have appsettings or 
        /// local.settings file to read from.
        /// ---------------------------------------------
        /// How to generate migration script:
        /// 1. Go to package manager console 
        /// 2. Set the Core project as default
        /// 3. Run 'add-migration' and provide a version name
        /// 4. Run 'update-database'
        /// </summary>
        public DataContext()
        {

        }

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<long>("PrimaryKeyGenerator").StartsAt(1000).IncrementsBy(1);

            modelBuilder.ApplyConfiguration(new UserBuilder());
            modelBuilder.ApplyConfiguration(new CarBuilder());
            modelBuilder.ApplyConfiguration(new OrderBuilder());
            modelBuilder.ApplyConfiguration(new OrderItemBuilder());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString  = _configuration?.GetConnectionString("dbConnectionString");
            if (connectionString == null)
            {
                // Used by the Migration script. The core project does not have appsettings or 
                // local.settings file to read from.
                connectionString = "Data Source=.;Initial Catalog=efcore;Integrated Security=true;Encrypt=false;";
            }

            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
