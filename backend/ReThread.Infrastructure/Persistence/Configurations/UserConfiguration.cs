using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReThreaded.Domain.Entities;

namespace ReThreaded.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table name (explicit > implicit)
        builder.ToTable("users");

        // Primary key (from BaseEntity)
        builder.HasKey(u => u.Id);

        // Email rules (mirror domain invariants)
        builder.Property(u => u.Email)
        .IsRequired()
        .HasMaxLength(256);

        builder.HasIndex(u => u.Email)
               .IsUnique();

        // Password hash must exist
        builder.Property(u => u.PasswordHash)
               .IsRequired();

        // Names
        builder.Property(u => u.FirstName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(u => u.LastName)
               .IsRequired()
               .HasMaxLength(100);

        // Enum safety
        builder.Property(u => u.Role)
               .HasConversion<string>()
               .IsRequired();

        // Optional fields
        builder.Property(u => u.ProfilePictureUrl)
               .HasMaxLength(500);

        // Status flag
        builder.Property(u => u.IsActive)
               .IsRequired();

        // Relationships
        builder.HasMany(u => u.Orders)
               .WithOne(o => o.User)
               .HasForeignKey(o => o.UserId);
    }
}
