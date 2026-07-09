using GymManagement.DbContexts;
using GymManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GymManagement.DAL.Date
{
    public static class GymDateSeed
    {
        public static async Task SeedAsync(GymDbContext gymDbContext,string seedFilePath ,ILogger logger, CancellationToken ct =default)
        {
			try
			{
				if(!await gymDbContext.Plans.AnyAsync(ct))
				{
					var plans = LoadDateFromFilePath<Plan>("Plans.json", seedFilePath);
					if(plans.Count > 0)
					{
						gymDbContext.Plans.AddRange(plans);
						logger.LogInformation($"Seeded {plans.Count} Succsses");

                    }
				}

				if (gymDbContext.ChangeTracker.HasChanges())
					await gymDbContext.SaveChangesAsync(ct);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Gym Date Seeding Failed");
				throw;
			}
        }

		private static List<T> LoadDateFromFilePath<T>(string fileName, string folderPath)
		{
			var filePath = Path.Combine(folderPath, fileName);
			if (!File.Exists(filePath))
				throw new FileNotFoundException($"Seeding Date File Not Found : {filePath}");

			var Date = File.ReadAllText(filePath);
			var Options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			Options.Converters.Add(new JsonStringEnumConverter());
			return JsonSerializer.Deserialize<List<T>>(Date, Options) ?? [];
        }
    }
}

