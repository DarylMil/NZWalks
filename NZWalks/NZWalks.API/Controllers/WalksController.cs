using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController:Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;


        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var AllWalks = await walkRepository.GetAllWalksAsync();
            var AllWalksDTO = mapper.Map<List<Models.DTO.Walk>>(AllWalks);
            return Ok(AllWalksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //Get Walk Domain object from db
            var walk = await walkRepository.GetWalkByIdAsync(id);
            //Convert Domain object to DTO
            if (walk != null)
            {
                var WalkDto = mapper.Map<Models.DTO.Walk>(walk);
                //Return response
                return Ok(WalkDto);
            }
            //Not Found response
            return NotFound("Invalid Walk Id. Please Try Again!");
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Convert DTO To Doman
            var walkDomain = new Walk()
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };
            //Pass Domain to Repository to persist
            walkDomain = await walkRepository.AddWalkAsync(walkDomain);
            //Convert Domain back to DTO
            var WalkDto = mapper.Map<Models.DTO.Walk>(walkDomain);
            //response back DTO
            return CreatedAtAction(nameof(GetWalkAsync),new { id = WalkDto.Id}, WalkDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            try
            {
                var WalkDomain = new Walk
                {
                    Length = updateWalkRequest.Length,
                    Name = updateWalkRequest.Name,
                    RegionId = updateWalkRequest.RegionId,
                    WalkDifficultyId = updateWalkRequest.WalkDifficultyId
                };
                var walk = await walkRepository.UpdateWalkAsync(id, WalkDomain);
                if (walk != null)
                {
                    var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
                    return Ok(walkDTO);
                }
                return NotFound();

            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return NotFound("Region Id or WalkDifficulty Id was not found.");
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync([FromRoute] Guid id)
        {
            var walk = await walkRepository.DeleteWalkByIdAsync(id);
            if (walk != null)
            {
                var walkDto = mapper.Map<Models.DTO.Walk>(walk);
                return Ok(walkDto);
            }
            return NotFound();
        }
    }
}
