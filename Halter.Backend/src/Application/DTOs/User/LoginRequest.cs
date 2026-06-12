using System.ComponentModel.DataAnnotations;
using Halter.Domain.Common;

namespace Halter.Application.DTOs;

public record class LoginRequest
{
    [Required(ErrorMessage = ErrorCodes.Required)]
    public string Email { get; init; } = null!;

    [Required(ErrorMessage = ErrorCodes.Required)]
    public string Password { get; init; } = null!;
}
