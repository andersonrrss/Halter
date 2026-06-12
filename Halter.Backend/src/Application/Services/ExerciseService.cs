using Halter.Application.DTOs;
using Halter.Application.Interfaces;
using Halter.Domain.Common;

namespace Halter.Application.Services;

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
