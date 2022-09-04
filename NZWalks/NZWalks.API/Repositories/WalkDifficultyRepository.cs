using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public WalkDifficultyRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }
        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = new Guid();
            await nZWalksDBContext.WalkDifficulty.AddAsync(walkDifficulty);
            await nZWalksDBContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var foundWalkDiff = await nZWalksDBContext.WalkDifficulty.FindAsync(id);
            if (foundWalkDiff != null)
            {
                nZWalksDBContext.WalkDifficulty.Remove(foundWalkDiff);
                await nZWalksDBContext.SaveChangesAsync();
            }
            return foundWalkDiff;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDBContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await nZWalksDBContext.WalkDifficulty.FindAsync(id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var foundWalkDiff = await nZWalksDBContext.WalkDifficulty.FindAsync(id);
            if (foundWalkDiff != null)
            {
                foundWalkDiff.Code = walkDifficulty.Code;
                await nZWalksDBContext.SaveChangesAsync();
            }
            return foundWalkDiff;
        }
    }
}
