using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
   public class PlatformsController : ControllerBase
   {
        private readonly ICommandRepository _repository;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepository repository,
        IMapper mapper)
       {
           _repository = repository;
           _mapper = mapper;
       }
       [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting All Platforms!");
            var platforms = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }
       [HttpPost]
       public ActionResult TestInbountConnection()
       {
           Console.WriteLine("--> Inbound Post # Command Service");
           return Ok("Inbound test of from Platforms Controller");
       }
   }

}