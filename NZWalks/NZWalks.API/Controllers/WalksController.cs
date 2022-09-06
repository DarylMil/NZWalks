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
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
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
            //Validate 
            if (!await ValidateAddWalkAsync(addWalkRequest))
            {
                return BadRequest(ModelState);
            }
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
            //Validate 
            if(! await ValidateUpdateWalkAsync(updateWalkRequest))
            {
                return BadRequest(ModelState);
            }
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
        #region private methods
        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if(addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), $"{nameof(addWalkRequest)} cannot be empty");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name),$"{nameof(addWalkRequest.Name)} cannot be empty.");
            }
            if(addWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} should be greater than zero");
            }

            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is invalid");
            }

            var walkDiff = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkDiff == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid");
            }

            if(ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest), $"{nameof(updateWalkRequest)} cannot be empty");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} cannot be empty.");
            }
            if (updateWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length), $"{nameof(updateWalkRequest.Length)} should be greater than zero");
            }

            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)} is invalid");
            }

            var walkDiff = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDiff == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId), $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
