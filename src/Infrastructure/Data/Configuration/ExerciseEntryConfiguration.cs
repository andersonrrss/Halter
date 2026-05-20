using GymApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymApp.Infrastructure;

public class ExerciseEntryConfiguration : IEntityTypeConfiguration<ExerciseEntry>
{
    public void Configure(EntityTypeBuilder<ExerciseEntry> builder)
    {
        builder.HasOne(ee => ee.WorkoutSession)
            .WithMany(ws => ws.ExerciseEntries)
            .HasForeignKey(ee => ee.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(ee => ee.Exercise)
            .WithMany()
            .HasForeignKey(ee => ee.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasIndex(ee => ee.CompletedAt);
    }
}
