using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using Shortener.Application.Common.Interfaces;
using Shortener.Application.Common.Options;
using Shortener.Application.Urls.Features.CreatingShortUrl.v1;
using Shortener.Domain.Entities;
using Shortener.Infrastructure.Persistence;

namespace Shortener.Application.UnitTests.Urls.Features.CreatingUrl.v1;

public class CreateUrlTests
{
    private Mock<IApplicationDbContext> _context;
    private List<Url> _urls;
    private Mock<IOptions<ApplicationOptions>> _options;

    [SetUp]
    public void SetUp()
    {
        _urls = Enumerable.Empty<Url>().ToList();
        _options = new Mock<IOptions<ApplicationOptions>>();
        _options.Setup(x => x.Value).Returns(new ApplicationOptions() { Uri = "https://localhost:5001" });
        _context = new Mock<IApplicationDbContext>();
        _context.Setup(x => x.Urls).ReturnsDbSet(_urls);
    }

    [Test]
    [TestCase("https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/",
        "0E01F1ED07")]
    public async Task ShouldAddEntityToDb(string url, string hash)
    {
        var handler = new CreateShortUrlHandler(_context.Object, _options.Object);
        var request = new CreateShortUrl(url);

        var result = await handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.BaseUrl.Should().BeEquivalentTo(url);
        result.ShortenedUrl.ToLower().Should().BeEquivalentTo(_options.Object.Value.Uri + "/" + hash.ToLower());
    }

    [Test]
    [TestCase("https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/",
        "0E01F1ED07")]
    public async Task ShouldThrowException(string url, string hash)
    {
        var handler = new CreateShortUrlHandler(_context.Object, _options.Object);
        var request = new CreateShortUrl(url);
        _urls.Add(new Url() { BaseUrl = url });

        var action = async () =>
        {
            await handler.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<Exception>().WithMessage("Url already exists.");
    }
}