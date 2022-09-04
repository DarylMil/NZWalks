using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository:IWalkRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public WalkRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }

        public async Task<Walk> AddWalkAsync(Walk walk)
        {
            walk.Id = new Guid();
            await nZWalksDBContext.Walks.AddAsync(walk);
            await nZWalksDBContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteWalkByIdAsync(Guid id)
        {
            var walk = await nZWalksDBContext.Walks.FindAsync(id);
            if (walk != null)
            {
                nZWalksDBContext.Walks.Remove(walk);
                await nZWalksDBContext.SaveChangesAsync();
            }
            return walk;
        }

        public async Task<List<Walk>> GetAllWalksAsync()
        {
            return await nZWalksDBContext
                .Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk> GetWalkByIdAsync(Guid id)
        {
            return await nZWalksDBContext
                .Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(walk => walk.Id == id);
        }

        public async Task<Walk> UpdateWalkAsync(Guid id, Walk walk)
        {
            var foundWalk = await nZWalksDBContext.Walks.FindAsync(id);
            if (foundWalk != null)
            {
                foundWalk.Name = walk.Name;
                foundWalk.Length = walk.Length;
                foundWalk.RegionId = walk.RegionId;
                foundWalk.WalkDifficultyId = walk.WalkDifficultyId;
                await nZWalksDBContext.SaveChangesAsync();
            }
            return foundWalk;
        }
    }
}
