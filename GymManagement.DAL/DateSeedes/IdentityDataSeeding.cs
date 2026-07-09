using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GymManagement.DAL.DateSeedes
{
    public static class IdentityDataSeeding
    {
        public async static Task SeedIdentityDataAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,ILogger logger ,CancellationToken ct = default)
        {
			try
			{
                bool hasUser = await userManager.Users.AnyAsync();
                bool hasRols = await roleManager.Roles.AnyAsync();
                if (!hasUser && hasRols) return;

                var roles = new List<IdentityRole>()
            {
                new IdentityRole("SuperAdmin"),
                new IdentityRole("Admin")
            };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name!))
                    {
                        var RoleResult = await roleManager.CreateAsync(role);
                        if (!RoleResult.Succeeded)
                        {
                            logger.LogError($"Failed To Create Role {role.Name} : {string.Join(";", RoleResult.Errors.Select(x => x.Description))}");
                        }
                    }
                }


                if (!hasUser)
                {
                    var usersToSeed = new List<(ApplicationUser User, string Password, string Role)>
                {
                    (
                    new ApplicationUser { FirstName = "Moaaz", LastName = "Atef", UserName = "MoaazAtef", Email = "moaazatef2020@gmail.com", PhoneNumber = "01153997317" },
                         "P@ssw0rd",
                         "SuperAdmin"
                         ),
                     (
                     new ApplicationUser { FirstName = "Belal", LastName = "Atef", UserName = "BelalAtef", Email = "Belalatef2020@gmail.com", PhoneNumber = "01153997319" },
                        "P@ssw0rd",
                        "Admin"
                        )
                };

                    foreach (var item in usersToSeed)
                    {
                        var addUserResult = await userManager.CreateAsync(item.User, item.Password);

                        if (!addUserResult.Succeeded)
                        {
                            logger.LogError($"Failed To Add User {item.User.UserName} : {string.Join(";", addUserResult.Errors.Select(x => x.Description))}");
                            continue;
                        }

                        var addRoleResult = await userManager.AddToRoleAsync(item.User, item.Role);

                        if (!addRoleResult.Succeeded)
                        {
                            logger.LogError($"Failed To Add Role To {item.User.UserName} : {string.Join(";", addRoleResult.Errors.Select(x => x.Description))}");
                        }
                    }

                    logger.LogInformation("IDentity Data Seeded");
                }

                return;
            }
			catch (Exception ex)
			{
                logger.LogError(ex, "Identity Seeding Failed");
				return;
			}
        }
    }
}

