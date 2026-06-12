using Microsoft.EntityFrameworkCore;
using Halter.Domain.Entities;
using System.Reflection;
using System.Text.Json;

namespace Halter.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; private set; }

    public DbSet<Routine> Routines { get; private set; }

    public DbSet<Exercise> Exercises { get; private set; }
    public DbSet<MuscleGroup> MuscleGroups { get; private set; }
    
    public DbSet<Workout> Workouts { get; private set; } 
    public DbSet<WorkoutExercise> WorkoutExercises { get; private set; }

    public DbSet<WorkoutSession> WorkoutSessions { get; private set; }
    public DbSet<ExerciseEntry> ExerciseEntries { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var muscleGroups = LoadSeedData<MuscleGroup>(
            "Halter.Infrastructure.Data.SeedData.MuscleGroups.json"
        );
        
        modelBuilder.Entity<MuscleGroup>().HasData(muscleGroups);

        var exercises = LoadSeedData<Exercise>(
            "Halter.Infrastructure.Data.SeedData.Exercises.json"
        );

        modelBuilder.Entity<Exercise>().HasData(exercises);
    }

    private List<T> LoadSeedData<T>(string resourceName)                                     
    {
        var assembly = Assembly.GetExecutingAssembly();
        using (var json = new StreamReader(assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException()))
        {
            if(json is null)
                throw new ArgumentNullException($"O arquivo de Seed Data {resourceName} não foi encontrado");

            return JsonSerializer.Deserialize<List<T>>(json.ReadToEnd())!;
        }
    }
}
