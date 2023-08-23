using Shortener.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Shortener.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Url> Urls { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}