using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Configurations
{
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(N => N.Name)
                    .HasColumnType("varchar(50)");
            builder.Property(N => N.Email)
                    .HasColumnType("varchar(100)");
            builder.Property(x => x.Phone)
                   .HasColumnType("varchar(11)");

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
                    
            builder.OwnsOne(x => x.Address, Address =>
            {
                Address.Property(x => x.BuildingNo).HasColumnName("BuildingNumber").HasColumnType("int");
                Address.Property(x => x.Street).HasColumnName("Street").HasColumnType("varchar(30)");
                Address.Property(x => x.City).HasColumnName("City").HasColumnType("varchar(30)");
                
            });
        }
    }
}
