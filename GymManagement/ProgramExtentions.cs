using GymManagement.DAL.Date;
using GymManagement.DAL.DateSeedes;
using GymManagement.DAL.Models;
using GymManagement.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.PL
{
    public static class ProgramExtentions
    {
        public static async Task MigrateAndSeedAsync( this WebApplication app) 
        {
            using var Scope = app.Services.CreateScope();
            var gymdbContext = Scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = Scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var roleManger = Scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManger = Scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var Configurations = Scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var pending = await gymdbContext.Database.GetPendingMigrationsAsync();
            if(pending.Any())
            {
                logger.LogInformation($"Apply {pending.Count()} Pending Migrations ");
                await gymdbContext.Database.MigrateAsync();
            }

            var seedPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDateSeed.SeedAsync(gymdbContext, seedPath, logger);
            await IdentityDataSeeding.SeedIdentityDataAsync(roleManger,userManger,logger);
        }
    }
}
