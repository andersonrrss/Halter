using GymApp.Application.DTOs;
using GymApp.Domain.Common;

namespace GymApp.Application.Interfaces;

public interface IExerciseService
{
    Task<Result<IEnumerable<ExerciseResponse>>> GetExercisesAsync(int page, int pageSize, int? muscleGroupId, string? search);
}
