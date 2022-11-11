using Microsoft.EntityFrameworkCore;
using MinimapApiTemplate.DAL.Model;

namespace MinimalApiTemplate.DAL
{
    public class GenericContext : DbContext
    {
        public GenericContext(DbContextOptions<GenericContext> options)
            : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}
