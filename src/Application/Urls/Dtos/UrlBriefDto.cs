using AutoMapper;
using Shortener.Application.Common.Mappings;
using Shortener.Domain.Entities;

namespace Shortener.Application.Urls.Dtos;

public class UrlBriefDto : IMapWith<Url>
{
    public Guid Id { get; set; }
    public string BaseUrl { get; set; } = null!;
    public string ShortenedUrl { get; set; } = null!;
    public bool IsDeletable { get; set; }
}

public class UrlDto : IMapWith<Url>
{
    public Guid Id { get; set; }
    public string BaseUrl { get; set; } = null!;
    public string ShortenedUrl { get; set; } = null!;
    public string Hash { get; set; }
    public DateTime Created { get; set; }
}