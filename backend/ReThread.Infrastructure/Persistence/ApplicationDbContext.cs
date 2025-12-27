using Microsoft.EntityFrameworkCore;
using ReThreaded.Domain.Entities;

namespace ReThreaded.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<DesignerProfile> DesignerProfiles => Set<DesignerProfile>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User -> DesignerProfile (One-to-One, Cascade)
        modelBuilder.Entity<DesignerProfile>()
            .HasOne(d => d.User)
            .WithOne(u => u.DesignerProfile)
            .HasForeignKey<DesignerProfile>(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // User -> Cart (One-to-One, Cascade)
        modelBuilder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithOne(u => u.Cart)
            .HasForeignKey<Cart>(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // User -> Orders (One-to-Many, Restrict)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // DesignerProfile -> Products (One-to-Many, Restrict)
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Designer)
            .WithMany(d => d.Products)
            .HasForeignKey(p => p.DesignerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Category -> Products (One-to-Many, Restrict)
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Product -> ProductImages (One-to-Many, Cascade)
        modelBuilder.Entity<ProductImage>()
            .HasOne(pi => pi.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Cart -> CartItems (One-to-Many, Cascade)
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Cart)
            .WithMany(c => c.Items)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        // CartItem -> Product (Many-to-One, Restrict)
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany()
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Order -> OrderItems (One-to-Many, Cascade)
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // OrderItem -> Product (Many-to-One, Restrict)
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // OrderItem -> Designer (Many-to-One, Restrict)
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Designer)
            .WithMany()
            .HasForeignKey(oi => oi.DesignerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly
        );
    }
}