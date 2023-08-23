using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shortener.Application.Urls.Features.CreatingShortUrl.v1;
using Shortener.Application.Urls.Features.GettingUrls.v1;
using Microsoft.AspNetCore.Http;
using Shortener.Application.Urls.Features.DeletingUrl.v1;

namespace Shortener.WebUI.Controllers;

public class UrlsController : ApiControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UrlsController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public async Task<ActionResult<GetUrlsResponse>> Get([FromQuery] GetUrls query)
    {
        return await Mediator.Send(query);
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(CreateShortUrlResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CreateShortUrlResponse>> Create([FromBody] CreateShortUrl command)
    {
        return await Mediator.Send(command);
    }

    [HttpDelete]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromBody] DeleteUrl command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
}