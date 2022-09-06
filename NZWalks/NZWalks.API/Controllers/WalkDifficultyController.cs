using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultyAsync()
        {
            var AllWalksDiff = await walkDifficultyRepository.GetAllAsync();
            //var regionsDTO = new List<Models.DTO.WalkDifficulty>();
            //AllWalksDiff.ToList().ForEach(x => regionsDTO.Add(new Models.DTO.WalkDifficulty()
            //{   
            //    Id = x.Id,
            //    Code = x.Code,
            //}));

            return Ok(mapper.Map<List<Models.DTO.WalkDifficulty>>(AllWalksDiff));
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync([FromRoute] Guid id)
        {
            var walkDiff = await walkDifficultyRepository.GetAsync(id);
            if (walkDiff != null)
            {
                return Ok(mapper.Map<Models.DTO.WalkDifficulty>(walkDiff));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody] Models.DTO.AddWalkDifficulty addWalkDifficulty)
        {
            //Validate 
            if (!ValidateAddWalkDifficulty(addWalkDifficulty))
            {
                return BadRequest(ModelState);
            }
            var newWalkDiff = new WalkDifficulty()
            {
                Code = addWalkDifficulty.Code,
            };
            var walkDiff = await walkDifficultyRepository.AddAsync(newWalkDiff);
            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new {id = walkDiff.Id}, mapper.Map<Models.DTO.WalkDifficulty>(walkDiff));   
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficulty updateWalkDifficulty)
        {
            //Validate 
            if (!ValidateUpdateWalkDifficulty(updateWalkDifficulty)){
                return BadRequest(ModelState);
            }

            var WalkDiffDom = new WalkDifficulty
            {
                Code = updateWalkDifficulty.Code,
            };
            var walkDiff = await walkDifficultyRepository.UpdateAsync(id, WalkDiffDom);
            if (walkDiff != null)
            {
                return Ok(mapper.Map<Models.DTO.WalkDifficulty>(walkDiff));
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync([FromRoute] Guid id)
        {
            var deletedWalkDiff = await walkDifficultyRepository.DeleteAsync(id);
            if(deletedWalkDiff != null)
            {
                return Ok(deletedWalkDiff);
            }
            return NotFound();
        }

        #region private methods
        private bool ValidateAddWalkDifficulty(Models.DTO.AddWalkDifficulty addWalkDifficulty)
        {
            if (string.IsNullOrWhiteSpace(addWalkDifficulty.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficulty.Code), $"{nameof(addWalkDifficulty.Code)} cannot be empty.");
                return false;
            }
            return true;
        }
        private bool ValidateUpdateWalkDifficulty(Models.DTO.UpdateWalkDifficulty updateWalkDifficulty)
        {
            if (string.IsNullOrWhiteSpace(updateWalkDifficulty.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficulty.Code), $"{nameof(updateWalkDifficulty.Code)} cannot be empty.");
                return false;
            }
            return true;
        }
        #endregion
    }
}
