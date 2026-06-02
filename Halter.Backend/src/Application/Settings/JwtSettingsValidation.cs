using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymApp.Application.Settings;

public static class JwtSettingsValidation
{
    public static ValidationResult ValidateSecret(string secret)
    {
        if((Encoding.UTF8.GetByteCount(secret) * 8) < 128)
            return new ValidationResult("'secret' não tem o número mínimo de bits(128)");

        return ValidationResult.Success!;
    }
}
