using AutoMapper;
using MinimapApiTemplate.Shared.Model;
using Models = MinimapApiTemplate.DAL.Model;

namespace MinimalApiTemplate.BLL.MapperProfiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<Models.City, City>()
                .ForMember(dst => dst.FullName, opt => opt.MapFrom(source => $"{source.Name}, {source.State}"))
                .ReverseMap();
        }
    }
}
