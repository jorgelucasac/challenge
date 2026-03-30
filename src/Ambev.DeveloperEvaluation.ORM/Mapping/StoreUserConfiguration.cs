using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class StoreUserConfiguration : IEntityTypeConfiguration<StoreUser>
{
    public void Configure(EntityTypeBuilder<StoreUser> builder)
    {
        builder.ToTable("StoreUsers");

        builder.HasKey(user => user.Id);
        builder.Property(user => user.Id)
            .UseIdentityByDefaultColumn();

        builder.Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(user => user.Username)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(user => user.Password)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(user => user.Phone)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(user => user.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(user => user.Role)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(user => user.CreatedAt)
            .IsRequired();

        builder.Property(user => user.UpdatedAt);

        builder.OwnsOne(user => user.Name, name =>
        {
            name.Property(item => item.Firstname)
                .HasColumnName("Firstname")
                .IsRequired()
                .HasMaxLength(100);

            name.Property(item => item.Lastname)
                .HasColumnName("Lastname")
                .IsRequired()
                .HasMaxLength(100);
        });

        builder.OwnsOne(user => user.Address, address =>
        {
            address.Property(item => item.City)
                .HasColumnName("City")
                .IsRequired()
                .HasMaxLength(100);

            address.Property(item => item.Street)
                .HasColumnName("Street")
                .IsRequired()
                .HasMaxLength(150);

            address.Property(item => item.Number)
                .HasColumnName("Number")
                .IsRequired();

            address.Property(item => item.Zipcode)
                .HasColumnName("Zipcode")
                .IsRequired()
                .HasMaxLength(50);

            address.OwnsOne(item => item.Geolocation, geolocation =>
            {
                geolocation.Property(location => location.Lat)
                    .HasColumnName("Latitude")
                    .IsRequired()
                    .HasMaxLength(50);

                geolocation.Property(location => location.Long)
                    .HasColumnName("Longitude")
                    .IsRequired()
                    .HasMaxLength(50);
            });
        });

        builder.HasIndex(user => user.Email);
        builder.HasIndex(user => user.Username);
    }
}
