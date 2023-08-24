using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using Shortener.Application.Common.Interfaces;
using Shortener.Application.Common.Options;
using Shortener.Application.Urls.Features.DeletingUrl.v1;
using Shortener.Domain.Entities;

namespace Shortener.Application.UnitTests.Urls.Features.DeletingUrl.v1;

public class DeleteUrlTests
{
    private List<Url> _urls;
    private Mock<IApplicationDbContext> _context;
    private Mock<ICurrentUserService> _currentUserService;

    [SetUp]
    public void SetUp()
    {
        _urls = new List<Url>() { };

        _currentUserService = new Mock<ICurrentUserService>();
        _context = new Mock<IApplicationDbContext>();
        _context.Setup(x => x.Urls).ReturnsDbSet(_urls);
    }


    [Test]
    public async Task UserTryToDeleteNotHisUrl_ShouldThrowUnauthorizedAccessException()
    {
        var userId = Guid.NewGuid().ToString();
        _currentUserService.Setup(x => x.UserId).Returns(userId);
        _currentUserService.Setup(x => x.Role).Returns("User");
        var url = new Url() { };
        _urls.Add(url);
        var command = new DeleteUrl(url.Id);
        var handler = new DeleteUrlHandler(_context.Object, _currentUserService.Object);

        var action = async () =>
        {
            await handler.Handle(command, CancellationToken.None);
        };

        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task UserTryToDeleteHisUrl_ShouldNotThrowUnauthorizedAccessException()
    {
        var userId = Guid.NewGuid().ToString();
        _currentUserService.Setup(x => x.UserId).Returns(userId);
        _currentUserService.Setup(x => x.Role).Returns("User");
        var url = new Url() { CreatedBy = userId };
        _urls.Add(url);
        var command = new DeleteUrl(url.Id);
        var handler = new DeleteUrlHandler(_context.Object, _currentUserService.Object);

        var action = async () =>
        {
            await handler.Handle(command, CancellationToken.None);
        };

        await action.Should().NotThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task AdminTryToDeleteHisUrl_ShouldNotThrowUnauthorizedAccessException()
    {
        var userId = Guid.NewGuid().ToString();
        _currentUserService.Setup(x => x.UserId).Returns(userId);
        _currentUserService.Setup(x => x.Role).Returns("Administrator");
        var url = new Url() { CreatedBy = userId };
        _urls.Add(url);
        var command = new DeleteUrl(url.Id);
        var handler = new DeleteUrlHandler(_context.Object, _currentUserService.Object);

        var action = async () =>
        {
            await handler.Handle(command, CancellationToken.None);
        };

        await action.Should().NotThrowAsync<UnauthorizedAccessException>();
    }
    
    [Test]
    public async Task AdminTryToDeleteNotHisUrl_ShouldNotThrowUnauthorizedAccessException()
    {
        var userId = Guid.NewGuid().ToString();
        _currentUserService.Setup(x => x.UserId).Returns(userId);
        _currentUserService.Setup(x => x.Role).Returns("Administrator");
        var url = new Url() { };
        _urls.Add(url);
        var command = new DeleteUrl(url.Id);
        var handler = new DeleteUrlHandler(_context.Object, _currentUserService.Object);

        var action = async () =>
        {
            await handler.Handle(command, CancellationToken.None);
        };

        await action.Should().NotThrowAsync<UnauthorizedAccessException>();
    }
}