using System.Text;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shortener.Application.Common.Interfaces;
using Shortener.Application.Urls.Dtos;
using Shortener.Domain.Entities;

namespace Shortener.Application.Urls.Features.CreatingShortUrl.v1;

public record CreateShortUrl(string Url) : IRequest<CreateShortUrlResponse>;

public class CreateShortUrlHandler : IRequestHandler<CreateShortUrl, CreateShortUrlResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;


    public CreateShortUrlHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
    {
        _context = context;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }


    public async Task<CreateShortUrlResponse> Handle(CreateShortUrl request, CancellationToken cancellationToken)
    {
        var encodedUrl = request.Url;
        var url = new Url() { BaseUrl = request.Url, ShortenedUrl = encodedUrl };

        await _context.Urls.AddAsync(url, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateShortUrlResponse();
    }
}

public record CreateShortUrlResponse();