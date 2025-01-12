using Shared;
using QueryableCore.DTOs;

namespace QueryableCore.RepositoriesInterfaces
{
    public interface IBuildingsRepository
    {
        int? CreateBuilding(BuildingDto buildingDto);
        List<BuildingDto> GetFilteredAndSortedBuildings(BuildingsRequestData requestData);
    }
}
