namespace ReThreaded.API.Contracts.Designers;

public class DesignerListItemResponse
{
    public Guid Id { get; init; }
    public string StoreName { get; init; } = null!;
    public string? Bio { get; init; }
    public string? BannerImageUrl { get; init; }
    public int TotalSales { get; init; }
    public decimal? AverageRating { get; init; }
    public int ProductCount { get; init; }
}

