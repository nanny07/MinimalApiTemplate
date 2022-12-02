using Microsoft.EntityFrameworkCore;
using MinimalApiTemplate.DAL.Configurations;
using MinimapApiTemplate.DAL.Model;

namespace MinimalApiTemplate.DAL
{
    public class GenericContext : DbContext
    {
        public GenericContext(DbContextOptions<GenericContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Add all the configurations in this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersonEntityTypeConfiguration).Assembly);
        }

        public DbSet<Person> People { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}
