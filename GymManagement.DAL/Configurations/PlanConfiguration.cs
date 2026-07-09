using GymManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Description)
                .HasMaxLength(200);

            builder.Property(p => p.Price)
                .HasPrecision(10, 2);

            builder.Property(d => d.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.ToTable(tp =>
            {
                tp.HasCheckConstraint("ck_plan_duration", "DurationDays BETWEEN 1 AND 365");
            });

            builder.Property(d => d.IsActive)
               .HasDefaultValue(true);
        }
    }
}
