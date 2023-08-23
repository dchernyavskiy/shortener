using MediatR;
using Microsoft.EntityFrameworkCore;
using Shortener.Application.Common.Interfaces;
using Shortener.Application.Urls.Dtos;
using Shortener.Application.Urls.Features.CreatingShortUrl.v1;

namespace Shortener.Application.Urls.Features.GettingUrls.v1;

public record GetUrls() : IRequest<GetUrlsResponse>;

public class GetUrlsHandler : IRequestHandler<GetUrls, GetUrlsResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly Dictionary<string, Func<Task<GetUrlsResponse>>> _strategies;

    public GetUrlsHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
        var admin = async () =>
        {
            var urls = await _context.Urls.Select(x => new UrlBriefDto()
            {
                BaseUrl = x.BaseUrl, Id = x.Id, IsDeletable = true, ShortenedUrl = x.ShortenedUrl
            }).ToListAsync();
            return new GetUrlsResponse(urls);
        };
        var user = async () =>
        {
            var userId = _currentUserService.UserId;
            var urls = await _context.Urls.Select(x => new UrlBriefDto()
            {
                BaseUrl = x.BaseUrl, Id = x.Id, IsDeletable = x.CreatedBy == userId, ShortenedUrl = x.ShortenedUrl
            }).ToListAsync();
            return new GetUrlsResponse(urls);
        };
        var anonymous = async () =>
        {
            var urls = await _context.Urls.Select(x => new UrlBriefDto()
            {
                BaseUrl = x.BaseUrl, Id = x.Id, IsDeletable = false, ShortenedUrl = x.ShortenedUrl
            }).ToListAsync();
            return new GetUrlsResponse(urls);
        };
        _strategies = new Dictionary<string, Func<Task<GetUrlsResponse>>>()
        {
            { "Admin", admin }, { "User", user }, { string.Empty, anonymous },
        };
    }

    public async Task<GetUrlsResponse> Handle(GetUrls request, CancellationToken cancellationToken)
    {
        var userRole = _currentUserService.Role;

        return await _strategies[userRole ?? string.Empty].Invoke();
    }
}

public record GetUrlsResponse(ICollection<UrlBriefDto> Urls);