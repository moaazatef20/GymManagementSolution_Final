using GymManagement.DAL.Models;
using GymManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DbContexts
{
    public class GymDbContext : IdentityDbContext<ApplicationUser>
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
            
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=GymDb;Trusted_Connection=True; TrustServerCertificate=true;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);

            modelBuilder.Entity<ApplicationUser>(ab =>
            {
                ab.Property(x => x.FirstName)
                  .HasColumnType("varchar")
                  .HasMaxLength(50);

                ab.Property(x => x.LastName)
                  .HasColumnType("varchar")
                  .HasMaxLength(50);
            });
        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Member> Members { get; set; }
        
    }
}
