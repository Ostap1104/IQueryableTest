using AutoMapper;
using MVCWebApp.Models;
using QueryableCore.DTOs;
namespace MVCWebApp.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BuildingDto, BuildingModel>();
            CreateMap<BuildingModel, BuildingDto>()
                .ForMember(buildingDto => buildingDto.Address, map => map.Ignore());
        }
    }

}

