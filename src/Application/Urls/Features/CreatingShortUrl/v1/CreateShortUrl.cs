using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shortener.Application.Common.Interfaces;
using Shortener.Domain.Entities;

namespace Shortener.Application.Urls.Features.CreatingShortUrl.v1;

public record CreateShortUrl(string Url) : IRequest<CreateShortUrlResponse>;

public class CreateShortUrlHandler : IRequestHandler<CreateShortUrl, CreateShortUrlResponse>
{
    private readonly IApplicationDbContext _context;


    public CreateShortUrlHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    private string GetSHA256Hash(string url)
    {
        using var hasher = SHA256.Create();
        return hasher.ComputeHash(Encoding.UTF8.GetBytes(url))
            .Aggregate("", (x, y) => x + y.ToString("x2"));
    }


    public async Task<CreateShortUrlResponse> Handle(CreateShortUrl request, CancellationToken cancellationToken)
    {
        if (await _context.Urls.AnyAsync(x => x.BaseUrl == request.Url, cancellationToken: cancellationToken))
        {
            throw new Exception("Url already exists.");
        }

        var encodedUrl = GetSHA256Hash(request.Url).Substring(0, 10);
        while (await _context.Urls.AnyAsync(x => x.ShortenedUrl == encodedUrl, cancellationToken: cancellationToken))
        {
            encodedUrl = GetSHA256Hash(encodedUrl);
        }

        var url = new Url() { BaseUrl = request.Url, ShortenedUrl = encodedUrl };


        await _context.Urls.AddAsync(url, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateShortUrlResponse(url.Id, url.BaseUrl, url.ShortenedUrl);
    }
}

public record CreateShortUrlResponse(Guid Id, string BaseUrl, string ShortenedUrl);