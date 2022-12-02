using AutoMapper;
using MinimapApiTemplate.Shared.Model;
using Models = MinimapApiTemplate.DAL.Model;

namespace MinimalApiTemplate.BLL.MapperProfiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Models.Person, Person>()
                .ForMember(dst => dst.FullName, opt => opt.MapFrom(source => $"{source.FirstName} {source.LastName}"))
                .ReverseMap();
        }
    }
}
 