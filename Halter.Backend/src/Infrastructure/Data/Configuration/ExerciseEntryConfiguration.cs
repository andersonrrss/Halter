using Halter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Halter.Infrastructure;

public class ExerciseEntryConfiguration : IEntityTypeConfiguration<ExerciseEntry>
{
    public void Configure(EntityTypeBuilder<ExerciseEntry> builder)
    {
        builder.HasOne(ee => ee.WorkoutSession)
            .WithMany(ws => ws.ExercisesEntries)
            .HasForeignKey(ee => ee.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(ee => ee.Exercise)
            .WithMany()
            .HasForeignKey(ee => ee.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasIndex(ee => ee.CompletedAt);
    }
}
