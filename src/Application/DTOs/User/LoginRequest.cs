using System.ComponentModel.DataAnnotations;

namespace GymApp.Application.DTOs;

public record class LoginRequest
{
    [Required(ErrorMessage ="Email é obrigatório")]
    public string Email { get; init; } = null!;

    [Required(ErrorMessage = "Senha é obrigatória")]
    public string Password { get; init; } = null!;
}
