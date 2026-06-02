using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GymApp.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? throw new UnauthorizedAccessException("Token inválido");

        return Guid.Parse(claim);
    }
}
