using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(cart => cart.Id);
        builder.Property(cart => cart.Id)
            .UseIdentityByDefaultColumn();

        builder.Property(cart => cart.UserId)
            .IsRequired();

        builder.Property(cart => cart.Date)
            .IsRequired();

        builder.Property(cart => cart.CreatedAt)
            .IsRequired();

        builder.Property(cart => cart.UpdatedAt);

        builder.Metadata.FindNavigation(nameof(Cart.Products))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(cart => cart.Products)
            .WithOne()
            .HasForeignKey(product => product.CartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
