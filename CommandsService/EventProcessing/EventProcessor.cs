using System;
using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory,
        IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
            }
        }
        private EventType DetermineEvent(string message)
        {
            Console.WriteLine("--> Determining event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(message);
            switch (eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("--> Platform published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Couldn't determine the event type");
                    return EventType.Undetermined;
            }
        }
        private void AddPlatform(string platformPublishedMessage)
        {
            Console.WriteLine("--> Adding platform");
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepository>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);
                
                try
                {
                    var platform = _mapper.Map<Platform>(platformPublishedDto);
                    if(!repo.ExternalPlatformExists(platform.ExternalId))
                    {
                        repo.CreatePlatform(platform);
                        repo.SaveChanges();
                    }
                    else{
                        Console.WriteLine("--> Platform already exists");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Error adding platform to DB: {ex.Message}");
                }
            }
        }

    }
    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}