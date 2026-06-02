using GymApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymApp.Infrastructure;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.Property(e => e.TimeConstraint).HasConversion<string>();
        builder.HasIndex(e => e.Name).IsUnique();
        builder.HasOne(e => e.MuscleGroup)
            .WithMany()
            .HasForeignKey(e => e.MuscleGroupId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
