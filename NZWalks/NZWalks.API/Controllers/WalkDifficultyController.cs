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
    }
}
