using System;
using System.Collections.Generic;
using CommandsService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.SyncDataServices.Grpc;

namespace CommandsService.Data
{
    public static class PrepareDatabase
    {
        public static void Prepare(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
                var platforms = grpcClient.ReturnAllPlatforms();
                SeedData(serviceScope.ServiceProvider.GetService<ICommandRepository>(),platforms);
            }
        }
        private static void SeedData(ICommandRepository repository, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("--> Seeding new platforms...");

            foreach (var platform in platforms)
            {
                if(!repository.ExternalPlatformExists(platform.Id))
                {
                    repository.CreatePlatform(platform);
                }
            }
            repository.SaveChanges();
            Console.WriteLine("--> Seeding new platforms... Done!");
        }

    }
}