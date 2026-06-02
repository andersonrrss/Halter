using System.ComponentModel.DataAnnotations;

namespace GymApp.Application.DTOs;

public record class RoutineRequest
{
    [Required(ErrorMessage = "É necessário um nome para a ficha de treino")]
    public string Name { get; init; } = null!;
}
