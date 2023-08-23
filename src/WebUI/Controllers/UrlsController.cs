﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shortener.Application.Urls.Features.CreatingShortUrl.v1;
using Shortener.Application.Urls.Features.GettingUrls.v1;
using Microsoft.AspNetCore.Http;

namespace Shortener.WebUI.Controllers;

public class UrlsController : ApiControllerBase
{
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
    public async Task<ActionResult<CreateShortUrlResponse>> Create([FromBody] CreateShortUrlDto command)
    {
        return await Mediator.Send(new CreateShortUrl(command.Url, Request.Host.Host));
    }
}