﻿using System.Security.Claims;
using Shortener.Application.Common.Interfaces;

namespace Shortener.WebUI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? Role => 
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
}