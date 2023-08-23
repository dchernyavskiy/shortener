using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shortener.Application.Common.Interfaces;
using Shortener.Application.Urls.Dtos;

namespace Shortener.Application.Urls.Features.GettingUrlDetails.v1;

public record GetUrlDetails(Guid Id) : IRequest<GetUrlDetailsResponse>;

public class GetUrlDetailsHandler : IRequestHandler<GetUrlDetails, GetUrlDetailsResponse>
{
    private readonly IApplicationDbContext _context;

    public GetUrlDetailsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetUrlDetailsResponse> Handle(GetUrlDetails request, CancellationToken cancellationToken)
    {
        var url = await _context.Urls.FirstOrDefaultAsync(x => x.Id == request.Id,
            cancellationToken: cancellationToken)!;
        return new GetUrlDetailsResponse(new UrlDto()
        {
            Id = url.Id,
            BaseUrl = url.BaseUrl,
            ShortenedUrl = url.ShortenedUrl,
            Hash = url.Hash,
            Created = url.Created
        });
    }
}

public record GetUrlDetailsResponse(UrlDto Url);