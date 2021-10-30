using System.Collections.Generic;
using CommandsService.Models;

namespace PlatformService.SyncDataServices.Grpc
{
    public interface IPlatformDataClient
    {
        IEnumerable<Platform> ReturnAllPlatforms();
    }
}