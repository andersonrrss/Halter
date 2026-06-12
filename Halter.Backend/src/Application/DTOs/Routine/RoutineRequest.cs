using System.ComponentModel.DataAnnotations;

namespace Halter.Application.DTOs;

public record class RoutineRequest
{
    [Required(ErrorMessage = "É necessário um nome para a ficha de treino")]
    public string Name { get; init; } = null!;
}
