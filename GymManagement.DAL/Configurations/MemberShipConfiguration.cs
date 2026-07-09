using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Configurations
{
    public class MemberShipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.Property(x => x.StartDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.HasOne(m => m.Member)
                .WithMany(m => m.Memberships)
                .HasForeignKey(m => m.MemberId);
                
            builder.HasOne(m => m.Plan)
                .WithMany(p => p.Memberships)
                .HasForeignKey(m => m.PlanId);

            //builder.HasKey(m => new { m.MemberId, m.PlanId });
            builder.HasKey(x => x.Id);

                
        }
    }
}
