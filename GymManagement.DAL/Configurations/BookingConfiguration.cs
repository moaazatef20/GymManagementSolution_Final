using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Property(x => x.CreatedAt)
                   .HasColumnName("BookingDate")
                   .HasDefaultValueSql("GETUTCDATE()")
                   .ValueGeneratedOnAdd();

            builder.HasIndex(x => x.Id);


            builder.HasOne(b => b.Member)
                    .WithMany(m => m.Bookings)
                    .HasForeignKey(b => b.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Session)
                    .WithMany(s => s.Bookings)
                    .HasForeignKey(b => b.SessionId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasKey(b => new { b.MemberId, b.SessionId });
                   
        }
    }
}
