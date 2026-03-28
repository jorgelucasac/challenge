using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(item => item.SaleId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(item => item.ProductExternalId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(item => item.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(item => item.Quantity)
            .IsRequired();

        builder.Property(item => item.UnitPrice)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(item => item.DiscountPercent)
            .IsRequired()
            .HasColumnType("numeric(5,2)");

        builder.Property(item => item.DiscountAmount)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(item => item.TotalAmount)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(item => item.IsCancelled)
            .IsRequired();

        builder.HasIndex(item => item.SaleId);
    }
}
