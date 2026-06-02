using System.ComponentModel.DataAnnotations;

namespace GymApp.Application.Settings;

public class JwtSettings
{
    [Required(ErrorMessage = "'Secret' não foi definido nas configurações, é necessário para o JWT")]
    [CustomValidation(typeof(JwtSettingsValidation), nameof(JwtSettingsValidation.ValidateSecret))]
    public string Secret { get; set; } = string.Empty;

    [Required(ErrorMessage = "O 'ExpireInMinutes' não foi definido nas configurações, é necessário para o JWT")]
    public int ExpiresInMinutes { get; set; }
    
    [Required(ErrorMessage = "O Issuer não foi definido nas configurações, é necessário para o JWT")]
    public string Issuer { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Audience não foi definido nas configurações, é necessário para o JWT")]
    public string Audience { get; set; } = string.Empty;
}
