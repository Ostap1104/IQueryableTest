using QueryableCore.DTOs;
using Shared;

namespace QueryableCore.Services.Interfaces
{
    public interface IBuildingsService
    {
        int? CreateBuilding(BuildingDto buildingDto);
        List<BuildingDto> GetBuildings(BuildingsRequestData requestData);
        List<string> GetRequiredFields(Type modelType);
        List<string> GetClassMembers(string modelName, bool? isRequired, AccessModifier[] accessModifiers, MemberType[] memberTypes, bool? isStatic);
    }
}
