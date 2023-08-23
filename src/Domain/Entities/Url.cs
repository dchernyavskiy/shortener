namespace Shortener.Domain.Entities;

public class Url : BaseAuditableEntity
{
    public string BaseUrl { get; set; } = null!;
    public string ShortenedUrl { get; set; } = null!;
}