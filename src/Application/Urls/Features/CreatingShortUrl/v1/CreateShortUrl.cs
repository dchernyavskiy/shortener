using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shortener.Application.Common.Interfaces;
using Shortener.Application.Common.Options;
using Shortener.Application.Urls.Exceptions;
using Shortener.Domain.Entities;

namespace Shortener.Application.Urls.Features.CreatingShortUrl.v1;

public record CreateShortUrl(string Url) : IRequest<CreateShortUrlResponse>;

public class CreateShortUrlHandler : IRequestHandler<CreateShortUrl, CreateShortUrlResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IOptions<ApplicationOptions> _options;

    public CreateShortUrlHandler(IApplicationDbContext context, IOptions<ApplicationOptions> options)
    {
        _context = context;
        _options = options;
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
            throw new UrlAlreadyExistsException();
        }


        var hash = GetSHA256Hash(request.Url).Substring(0, 10);
        var host = _options.Value.Uri.Trim('/') + "/" + hash;
        while (await _context.Urls.AnyAsync(x => x.ShortenedUrl == host,
                   cancellationToken: cancellationToken))
        {
            hash = GetSHA256Hash(request.Url).Substring(0, 10);
            host = _options.Value.Uri.Trim('/') + "/" + hash;
        }

        var url = new Url() { BaseUrl = request.Url, Hash = hash, ShortenedUrl = host };

        await _context.Urls.AddAsync(url, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateShortUrlResponse(url.Id, url.BaseUrl, url.ShortenedUrl);
    }
}

public record CreateShortUrlResponse(Guid Id, string BaseUrl, string ShortenedUrl);