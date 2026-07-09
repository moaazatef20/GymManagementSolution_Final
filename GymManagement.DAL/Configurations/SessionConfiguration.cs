using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.HasCheckConstraint("CK_Session_Capacity", "[Capacity] >= 1 AND [Capacity] <= 25");
            builder.HasCheckConstraint("CK_Session_Dates", "[StartDate] < [EndDate]");

            builder.HasOne(x => x.Category)
                   .WithMany(c => c.Sessions)
                   .HasForeignKey(x => x.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Trainer)
                  .WithMany(t => t.Sessions)
                  .HasForeignKey(x => x.TrainerId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
