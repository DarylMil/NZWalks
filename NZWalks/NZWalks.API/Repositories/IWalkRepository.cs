using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<List<Walk>> GetAllWalksAsync();
        Task<Walk> GetWalkByIdAsync(Guid id);
        Task<Walk> AddWalkAsync(Walk walk);
        Task<Walk> DeleteWalkByIdAsync(Guid id);
        Task<Walk> UpdateWalkAsync(Guid id, Walk walk);
    }
}
