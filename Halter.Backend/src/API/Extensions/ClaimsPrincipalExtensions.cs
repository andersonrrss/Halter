using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Halter.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? throw new UnauthorizedAccessException("Token inválido");

        return Guid.Parse(claim);
    }
}
