using ReThreaded.Domain.Common;
using ReThreaded.Domain.Exceptions;

namespace ReThreaded.Domain.Entities;

public class DesignerProfile : BaseEntity
{
    public Guid UserId { get; private set; }
    public string StoreName { get; private set; }
    public string? Bio { get; private set; }
    public string? BannerImageUrl { get; private set; }
    public string? InstagramHandle { get; private set; }
    public string? WebsiteUrl { get; private set; }
    public int TotalSales { get; private set; }
    public decimal? AverageRating { get; private set; }

    // Navigation
    public User User { get; private set; }
    public ICollection<Product> Products { get; private set; }

    private DesignerProfile()
    {
        Products = new List<Product>();
    }

    public static DesignerProfile Create(Guid userId, string storeName, string? bio)
    {
        if (userId == Guid.Empty)
            throw new DomainException("User ID is required");

        if (string.IsNullOrWhiteSpace(storeName))
            throw new DomainException("Store name is required");

        if (storeName.Length > 100)
            throw new DomainException("Store name too long (max 100 chars)");

        return new DesignerProfile
        {
            UserId = userId,
            StoreName = storeName.Trim(),
            Bio = bio?.Trim(),
            TotalSales = 0,
            AverageRating = null
        };
    }

    public void UpdateProfile(string storeName, string? bio, string? instagramHandle, string? websiteUrl)
    {
        if (string.IsNullOrWhiteSpace(storeName))
            throw new DomainException("Store name is required");

        StoreName = storeName.Trim();
        Bio = bio?.Trim();
        InstagramHandle = instagramHandle?.Trim();
        WebsiteUrl = websiteUrl?.Trim();
        SetUpdatedAt();
    }

    public void SetBanner(string bannerUrl)
    {
        BannerImageUrl = bannerUrl;
        SetUpdatedAt();
    }

    public void IncrementSales()
    {
        TotalSales++;
        SetUpdatedAt();
    }

    public void UpdateRating(decimal newAverageRating)
    {
        if (newAverageRating < 0 || newAverageRating > 5)
            throw new DomainException("Rating must be between 0 and 5");

        AverageRating = newAverageRating;
        SetUpdatedAt();
    }
}