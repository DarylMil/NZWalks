using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync();
            //retrun DTO regions
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat, 
            //        Long = region.Long,
            //        Population = region.Population,
            //    };
            //    regionsDTO.Add(regionDTO);
            //});

            //convert from Domain to DTO
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")] //Route is basically the /regions. Then {id:guid} means, id params restricted to only guid
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync([FromRoute] Guid id)
        {
            var region = await regionRepository.GetAsync(id);

            if (region == null)
            {
                return NotFound();
            }
            var regionDTO = mapper.Map<Models.DTO.Region>(region); // <Dest>(source)
            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync([FromBody] Models.DTO.AddRegionRequest newRegion)
        {
            //Request to Domain Model
            var region = new Region()
            {
                Code = newRegion.Code,
                Area = newRegion.Area,
                Lat = newRegion.Lat,
                Long = newRegion.Long,
                Name = newRegion.Name,
                Population = newRegion.Population
            };
            //Pass details to Repository
            region = await regionRepository.AddAsync(region);

            //Convert Back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };
            //CreatedAtAction shows that its created and which API you can use to find the newly created resource, including the params
            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id}, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync([FromRoute] Guid id)
        {
            //Get region from database
            var region = await regionRepository.DeleteAsync(id);
            //if null no such region
            if (region == null)
            {
                return NotFound();
            }
            //convert response to DTO
            //var regionDTO = new Models.DTO.Region()
            //{
            //    Id = region.Id,
            //    Code = region.Code,
            //    Area = region.Area,
            //    Lat = region.Lat,
            //    Long = region.Long,
            //    Name = region.Name,
            //    Population = region.Population,
            //};

            // use mapper to convert
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            //return OK response
            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            // Convert DTO to Domain
            var newRegion = new Region()
            {
                Id = id,
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population
            };
            // Update Region using repository
            var region = await regionRepository.UpdateAsync(id, newRegion);
            // if Null then not found
            if (region == null)
            {
                return NotFound();
            }
            // convert Domain back to DTO
            var RegionDTO = mapper.Map<Models.DTO.Region>(region);
            // ok response
            return Ok(RegionDTO);
        }
    }
}
