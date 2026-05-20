using GymApp.Application;
using GymApp.Application.Interfaces;
using GymApp.Application.Services;
using GymApp.Infrastructure;
using GymApp.Infrastructure.Repositories;
using GymApp.Infrastructure.Services;
using Microsoft.OpenApi;

namespace GymApp.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoutineRepository, RoutineRepository>();
        services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<IWorkoutExerciseRepository, WorkoutExerciseRepository>();
        services.AddScoped<IWorkoutSessionRepository, WorkoutSessionRepository>();
        services.AddScoped<IExerciseEntryRepository, ExerciseEntryRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IHashPasswordService, HashPasswordService>();
        services.AddScoped<IAuthService, AuthService>(); 
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoutineService, RoutineService>();
        services.AddScoped<IWorkoutService, WorkoutService>();
        services.AddScoped<IExerciseService, ExerciseService>();
        services.AddScoped<IWorkoutExerciseService, WorkoutExerciseService>();
        services.AddScoped<IWorkoutSessionService, WorkoutSessionService>();
        services.AddScoped<IExerciseEntryService, ExerciseEntryService>();
        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "1.0",
                Title = "Gym-app",
                Description = "Um app que armazena e organiza fichas de treinos"
            });

            options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Autorização do JWT no Header usando o Bearer Scheme"
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("bearer", document)] = []
            });            
        });

        return services;
    }
}
