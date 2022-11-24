using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimapApiTemplate.DAL.Model;
using System.Reflection.Metadata;

namespace MinimalApiTemplate.DAL.Configurations
{
    internal class CityEntityTypeConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .IsRequired();

            builder
                .Property(p => p.Name)
                .HasMaxLength(30)
                .IsRequired();
                
            builder
                .Property(p => p.State)
                .HasMaxLength(2)
                .IsRequired();

            builder.HasData(new List<City>() {
                new City() { Id = Guid.NewGuid(), Name = "Arezzo", State = "IT" },
                new City() { Id = Guid.NewGuid(), Name = "Firenze", State = "IT" }
            });
        }
    }
}
