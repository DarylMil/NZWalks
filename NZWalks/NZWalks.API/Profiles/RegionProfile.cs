using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class RegionProfile:Profile
    {
        public RegionProfile()
        {
            CreateMap<Models.Domain.Region, Models.DTO.Region>().ForMember(dest => dest.Id,
                options => options.MapFrom(src => src.Id));

        }
    }
}
