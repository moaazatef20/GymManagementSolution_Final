
using GymManagement.BLL.Service.Classes;
using GymManagement.BLL.Service.InterFaces;
using GymManagement.BLL.ViewModels.MembersVIewModels;


using GymManagement.BLL.Service.Classes;
using GymManagement.BLL.Service.InterFaces;
using GymManagement.BLL.ViewModels.MembersVIewModels;
using GymManagement.BLL.ViewModels.MembersVIewModels;
using GymManagement.DAL.Repositorities.Classes;
using GymManagement.DAL.Repositorities.Interfaces;
using GymManagement.DbContexts;
using Microsoft.EntityFrameworkCore;
using GymManagement.BLL.Utilities;
using GymManagement.PL;
using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Identity;


namespace GymManagement
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();

            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IMemberServices, MemberServices>();
            builder.Services.AddScoped<ISessionServices, SessionServices>();
            builder.Services.AddScoped<ITrainerServices,TrainerService>();
            builder.Services.AddScoped<IPlanServices, PlanServices>();
            builder.Services.AddScoped<IDashBoardServices, DashBoardServices>();
            builder.Services.AddScoped<IAttachementServices, AttachementService>();
            builder.Services.AddScoped<IMembershipServices, MembershipServices>();
            builder.Services.AddScoped<IBookingServices, BookingServices>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                config.Lockout.MaxFailedAccessAttempts = 3;
            })
                .AddEntityFrameworkStores<GymDbContext>();





            builder.Services.AddDbContext<GymDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            var app = builder.Build();
            await app.MigrateAndSeedAsync();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
