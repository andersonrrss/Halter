using System.ComponentModel.DataAnnotations;
using Halter.Domain.Common;

namespace Halter.Application.DTOs;

public record class RegisterRequest
{
    [Required(ErrorMessage = ErrorCodes.Required, AllowEmptyStrings = false)]
    [MinLength(3, ErrorMessage = ErrorCodes.TooShort)]
    [MaxLength(50, ErrorMessage = ErrorCodes.TooLong)]
    public string Name { get; init; } = null!;

    [Required(ErrorMessage = ErrorCodes.Required)]
    [EmailAddress(ErrorMessage = ErrorCodes.Invalid)]
    public string Email { get; init; } = null!;

    [Required(ErrorMessage = ErrorCodes.Required)]
    [MinLength(6, ErrorMessage = ErrorCodes.TooShort)]
    [RegularExpression(@"^\S+$", ErrorMessage = ErrorCodes.Invalid)]
    public string Password { get; init; } = null!;
}                               