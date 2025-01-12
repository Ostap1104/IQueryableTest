using QueryableCore.DTOs;
using QueryableDatabase.Models;
using AutoMapper;
namespace QueryableDatabase.Mapping
{
    public class QueryableDatabaseMapperProfile : Profile
    {
        public QueryableDatabaseMapperProfile()
        {
            CreateMap<Building, BuildingDto>().ReverseMap();
            //CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
