using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class WalkProfile : Profile
    {
        public WalkProfile()
        {
            CreateMap<Models.Domain.Walk, Models.DTO.Walk>();
            CreateMap<Models.Domain.WalkDifficulty, Models.DTO.WalkDifficulty>();
        }
    }
}
