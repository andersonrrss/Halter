using System.ComponentModel.DataAnnotations;

namespace GymApp.Application.DTOs;

public record class RegisterRequest
{
    [Required(ErrorMessage = "Nome é obrigatório", AllowEmptyStrings = false)]
    [MinLength(3, ErrorMessage = "Nome deve conter pelo menos três caracteres")]
    [MaxLength(50, ErrorMessage = "Nome deve conter no máximo 50 caracteres")]
    public string Name { get; init; } = null!;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; init; } = null!;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve conter pelo menos 6 caracteres")]
    [RegularExpression(@"^\S+$", ErrorMessage = "A senha não pode conter espaços em branco")]
    public string Password { get; init; } = null!;
}                               