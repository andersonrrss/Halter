using System.Text;
using GymApp.Application.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace GymApp.API.Extensions;

public static class JwtAuthExtension
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        JwtSettings settings
    )
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            // Desativa o mapeamento automático do ASP.NET que renomeia os claims do JWT ao validar o token.
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret)),
                ValidIssuer = settings.Issuer,
                ValidAudience = settings.Audience,

                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true
            };
        });

        return services;
    }
}
