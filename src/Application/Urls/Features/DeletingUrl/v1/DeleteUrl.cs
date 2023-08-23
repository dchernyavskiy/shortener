using MediatR;
using Microsoft.EntityFrameworkCore;
using Shortener.Application.Common.Exceptions;
using Shortener.Application.Common.Interfaces;
using Shortener.Domain.Entities;

namespace Shortener.Application.Urls.Features.DeletingUrl.v1;

public record DeleteUrl(Guid Id) : IRequest;

public class DeleteUrlHandler : IRequestHandler<DeleteUrl>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly Dictionary<string, Func<Url, CancellationToken, Task>> _strategies;

    public DeleteUrlHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;

        var delete = async (Url url, CancellationToken cancellationToken) =>
        {
            _context.Urls.Remove(url);
            await _context.SaveChangesAsync(CancellationToken.None);
        };
        var user = async (Url url, CancellationToken cancellationToken) =>
        {
            var userId = _currentUserService.UserId;
            if (url.CreatedBy != userId)
            {
                throw new UnauthorizedAccessException();
            }

            await delete(url, cancellationToken);
        };
        _strategies = new Dictionary<string, Func<Url, CancellationToken, Task>>()
        {
            { "Admin", delete }, { "User", user }
        };
    }

    public async Task Handle(DeleteUrl request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var userRole = _currentUserService.Role;
        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var url = await _context.Urls.FirstOrDefaultAsync(x => x.Id == request.Id,
            cancellationToken: cancellationToken);

        if (url == null)
        {
            throw new NotFoundException("Url was not found.");
        }

        await _strategies[userRole!].Invoke(url, cancellationToken);
    }
}