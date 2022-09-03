using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public RegionRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var isFound = await nZWalksDBContext.Regions.FirstOrDefaultAsync(region => region.Id == id);
            if (isFound != null)
            {
                //Delete the Region
                nZWalksDBContext.Regions.Remove(isFound);
                await nZWalksDBContext.SaveChangesAsync();
                return isFound;
            }
            return null;
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await nZWalksDBContext.AddAsync(region);
            await nZWalksDBContext.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nZWalksDBContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await nZWalksDBContext.Regions.FirstOrDefaultAsync(region => region.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var foundRegion = await nZWalksDBContext.Regions.FirstOrDefaultAsync(region => region.Id == id);
            if (foundRegion != null )
            {
                foundRegion.Code = region.Code;
                foundRegion.Name = region.Name;
                foundRegion.Area = region.Area;
                foundRegion.Lat = region.Lat;
                foundRegion.Long = region.Long;
                foundRegion.Population = region.Population;

                await nZWalksDBContext.SaveChangesAsync();
                return foundRegion;
            }
            return null;
        }
    }
}
