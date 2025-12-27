using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReThreaded.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReThread.Infrastructure.Persistence.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            // Primary Key
            builder.HasKey(ci => ci.Id);

            // Cart → CartItems (Owner relationship)
            builder.HasOne(ci => ci.Cart)
                   .WithMany(c => c.Items)
                   .HasForeignKey(ci => ci.CartId)
                   .OnDelete(DeleteBehavior.Cascade); // ✅ OK

            // Product → CartItems (Reference only)
            builder.HasOne(ci => ci.Product)
                   .WithMany()
                   .HasForeignKey(ci => ci.ProductId)
                   .OnDelete(DeleteBehavior.Restrict); // 🔥 IMPORTANT FIX

            // Optional: table name (clean & explicit)
            builder.ToTable("CartItems");
        }
    }
}