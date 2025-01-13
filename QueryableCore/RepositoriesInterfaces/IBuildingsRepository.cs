using Shared;
using QueryableCore.DTOs;

namespace QueryableCore.RepositoriesInterfaces
{
    public interface IBuildingsRepository
    {
        int? CreateBuilding(BuildingDto buildingDto);
        List<BuildingDto> GetFilteredAndSortedBuildings(BuildingsRequestData requestData);
        BuildingDto? Get(int id);
        Task<bool> UpdateBuildingAsync(BuildingDto buildingDto);
        Task<bool> DeleteAsync(int id);
    }
}
