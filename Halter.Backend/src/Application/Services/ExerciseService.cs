using GymApp.Application.DTOs;
using GymApp.Application.Interfaces;
using GymApp.Domain.Common;

namespace GymApp.Application.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;

    public ExerciseService(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<Result<IEnumerable<ExerciseResponse>>> GetExercisesAsync(
        int page, 
        int pageSize, 
        int? muscleGroupId,
        string? search
        )
    {
        var exerciseList = await _exerciseRepository.GetAllAsync(page, pageSize, search, muscleGroupId);

        return Result<IEnumerable<ExerciseResponse>>.Success(
            exerciseList.Select(ExerciseResponse.FromEntity)
        );
    }
}
