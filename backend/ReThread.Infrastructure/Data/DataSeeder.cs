using ReThreaded.Domain.Entities;
using ReThreaded.Domain.Enums;
using ReThreaded.Infrastructure.Persistence;

namespace ReThreaded.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        // Only seed if database is empty
        if (context.Users.Any()) return;

        // 1. Create Categories
        var categories = new List<Category>
        {
            Category.Create("Tops", "T-shirts, blouses, sweaters", 1),
            Category.Create("Bottoms", "Jeans, pants, skirts", 2),
            Category.Create("Outerwear", "Jackets, coats, hoodies", 3),
            Category.Create("Dresses", "Dresses and jumpsuits", 4),
            Category.Create("Accessories", "Bags, hats, scarves", 5)
        };
        context.Categories.AddRange(categories);
        context.SaveChanges();

        // 2. Create Test Users
        var buyer = User.Create(
            "buyer@test.com",
            "hashed_password_123", // In real app, hash this!
            "Alice",
            "Johnson",
            UserRole.Buyer
        );

        var designerUser = User.Create(
            "designer@test.com",
            "hashed_password_123",
            "Bob",
            "Smith",
            UserRole.Designer
        );

        var admin = User.Create(
            "admin@test.com",
            "hashed_password_123",
            "Admin",
            "User",
            UserRole.Admin
        );

        context.Users.AddRange(buyer, designerUser, admin);
        context.SaveChanges();

        // 3. Create Designer Profile
        var designerProfile = DesignerProfile.Create(
            designerUser.Id,
            "Vintage Vibes Studio",
            "Transforming thrift finds into unique fashion statements. Specializing in denim and vintage tees."
        );
        context.DesignerProfiles.Add(designerProfile);
        context.SaveChanges();

        // 4. Create Sample Products
        var jacket = Product.Create(
            "Vintage Levi's Embroidered Jacket",
            "Classic denim jacket transformed with hand-embroidered wildflowers across the back and shoulders. Reinforced buttons and custom leather elbow patches.",
            "Found this beauty at a local thrift store for $8. Spent 20 hours hand-embroidering the floral design using vintage thread. Added leather patches to give it character while maintaining authenticity.",
            75.00m,
            "M",
            ProductCondition.Excellent,
            designerProfile.Id,
            categories[2].Id,
            1,// Outerwear
            "Levi's"
        );

        var tshirt = Product.Create(
            "Upcycled Band Tee Crop Top",
            "Oversized vintage band tee transformed into a flattering crop top with custom bleach art and distressed edges.",
            "Started with an oversized Nirvana tee from the 90s. Created unique bleach splatter art, then carefully cut and hemmed into a modern crop silhouette. Safety pin Primarys add edge.",
            35.00m,
            "S",
            ProductCondition.Good,
            designerProfile.Id,
            categories[0].Id,
            1,// Tops
            "Vintage Band Tee"
        );

        var jeans = Product.Create(
            "High-Waist Patchwork Jeans",
            "Classic mom jeans redesigned with colorful fabric patches and hand-stitched Primarys.",
            "Took a pair of thrifted Levi's 501s and added unique fabric patches from vintage quilts. Each patch tells a story. Hand-stitched for durability.",
            55.00m,
            "L",
            ProductCondition.Excellent,
            designerProfile.Id,
            categories[1].Id,
            1,// Bottoms
            "Levi's"
        );

        context.Products.AddRange(jacket, tshirt, jeans);
        context.SaveChanges();

        // 5. Add Product Images (placeholder URLs)
        jacket.AddImage("https://placehold.co/600x400/8B9E7D/ffffff?text=Gallery+Jacket", ProductImageType.Gallery, 1);
        jacket.AddImage("https://placehold.co/600x400/C89D7C/ffffff?text=Primary+Jacket", ProductImageType.Primary, 2);
        jacket.AddImage("https://placehold.co/600x400/E8DCC4/ffffff?text=Primary+Embroidery", ProductImageType.Primary, 3);

        tshirt.AddImage("https://placehold.co/600x400/8B9E7D/ffffff?text=Gallery+Tee", ProductImageType.Gallery, 1);
        tshirt.AddImage("https://placehold.co/600x400/C89D7C/ffffff?text=Primary+Tee", ProductImageType.Primary, 2);

        jeans.AddImage("https://placehold.co/600x400/8B9E7D/ffffff?text=Gallery+Jeans", ProductImageType.Gallery, 1);
        jeans.AddImage("https://placehold.co/600x400/C89D7C/ffffff?text=Primary+Jeans", ProductImageType.Primary, 2);

        context.SaveChanges();

        Console.WriteLine("✅ Database seeded successfully!");
    }
}