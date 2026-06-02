using GymApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymApp.Infrastructure;

public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.HasOne(w => w.Routine)
            .WithMany(r => r.Workouts)
            .HasForeignKey(w => w.RoutineId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
