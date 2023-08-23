using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Shortener.Infrastructure.Identity;

public class ProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);


        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            context.IssuedClaims.Add(new Claim(ClaimTypes.Role, role));
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, role));
        }

        context.AddRequestedClaims(context.Subject.Claims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);

        context.IsActive = (user != null);
    }
}