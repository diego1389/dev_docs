using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OdeToFood.Core;

namespace OdeToFood.Data
{
    public class OdeToFoodConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public OdeToFoodConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.HasKey(prop => prop.Id);
            builder.Property(prop => prop.Name)
                .HasMaxLength(80)
                .IsRequired();
            builder.Property(prop => prop.Location)
                .HasMaxLength(256)
                .IsRequired();
            builder.Property(prop => prop.Cuisine)
                .IsRequired();
        }
    }
}
