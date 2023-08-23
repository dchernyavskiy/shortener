using Shortener.Application.Common.Interfaces;

namespace Shortener.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
