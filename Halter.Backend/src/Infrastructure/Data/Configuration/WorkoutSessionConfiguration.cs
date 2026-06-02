using GymApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymApp.Infrastructure;

public class WorkoutSessionConfiguration : IEntityTypeConfiguration<WorkoutSession>
{
    public void Configure(EntityTypeBuilder<WorkoutSession> builder)
    {
        builder.HasOne(ws => ws.Workout)
            .WithMany()
            .HasForeignKey(ws => ws.WorkoutId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(ws => ws.User)
            .WithMany()
            .HasForeignKey(ws => ws.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
