using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Persistence
{
    public class PharmacyContextSeed
    {
        public static async Task SeedRolesAsync(PharmacyDbContext context, ILoggerFactory loggerFactory)
    {
        try
        {
            if (!context.Roles.Any())
            {
                var roles = new List<Rol>()
                        {
                            new Rol{Id=1, Name="Administrator"},
                            new Rol{Id=2, Name="Employee"},
                            new Rol{Id=3, Name="Patient"},
                            new Rol{Id=4, Name="WithoutRol"},
                        };
                context.Roles.AddRange(roles);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<PharmacyDbContext>();
            logger.LogError(ex.Message);
        }
    }
    }
}