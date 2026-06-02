using GymApp.Application.Interfaces;
using GymApp.Domain.Entities;
using GymApp.Application.Settings;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Claims;

namespace GymApp.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly JwtSettings _settings;

    public JwtService(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }

    public string Generate(User user)
    {
        var handler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_settings.Secret);

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
            );

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(user),
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes),
            IssuedAt = DateTime.UtcNow,
            Issuer = _settings.Issuer,
            Audience = _settings.Audience
        };

        var token = handler.CreateToken(descriptor);

        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var ci = new ClaimsIdentity();

        ci.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        ci.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
        ci.AddClaim(new Claim(JwtRegisteredClaimNames.Email, user.Email));

        return ci;
    }
}
