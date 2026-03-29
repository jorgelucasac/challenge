using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(product => product.Id);

        builder.Property(product => product.Id)
            .UseIdentityByDefaultColumn();

        builder.Property(product => product.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(product => product.Price)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(product => product.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(product => product.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(product => product.Image)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(product => product.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(product => product.UpdatedAt)
            .HasColumnType("timestamp with time zone");

        builder.OwnsOne(product => product.Rating, rating =>
        {
            rating.Property(value => value.Rate)
                .HasColumnName("RatingRate")
                .HasColumnType("numeric(5,2)")
                .IsRequired();

            rating.Property(value => value.Count)
                .HasColumnName("RatingCount")
                .IsRequired();
        });

        builder.HasIndex(product => product.Category);
        builder.HasIndex(product => product.Title);
    }
}
