using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimapApiTemplate.DAL.Model;

namespace MinimalApiTemplate.DAL.Configurations
{
    internal class PersonEntityTypeConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.FirstName)
                .HasMaxLength(30)
                .IsRequired();

            builder
                .Property(p => p.LastName)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasData(new List<Person>() {
                new Person() { Id = Guid.NewGuid(), FirstName = "Andrea", LastName = "Rossi" },
                new Person() { Id = Guid.NewGuid(), FirstName = "Gianni", LastName = "Verdi" }
            });
        }
    }
}
