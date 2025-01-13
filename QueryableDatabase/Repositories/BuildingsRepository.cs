using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QueryableCore.DTOs;
using QueryableCore.RepositoriesInterfaces;
using QueryableDatabase.Migrations;
using QueryableDatabase.Models;
using Shared;

namespace QueryableDatabase.Repositories
{
    public class BuildingsRepository : IBuildingsRepository
    {
        private readonly MsSqlContext _dbContext;
        private readonly IMapper _mapper;
    
        public BuildingsRepository(MsSqlContext msSqlContext, IMapper mapper)
        {
            _dbContext = msSqlContext;
            _mapper = mapper;
        }

        public int? CreateBuilding(BuildingDto buildingDto)
        {
            Building building = _mapper.Map<Building>(buildingDto);
           

            _dbContext.Buildings.Add(building);

            var request = _dbContext.Buildings.Where(b => b.Id == building.Id);

            int changesCount = _dbContext.SaveChanges();

            return changesCount > 0 ? building.Id : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Building? building = await _dbContext.Buildings.FirstOrDefaultAsync(b => b.Id == id);

            if (building == null)
            {
                return false;
            }

            _dbContext.Buildings.Remove(building);
            int countDeleted = await _dbContext.SaveChangesAsync();
            return countDeleted > 0;
        }

        public BuildingDto? Get(int id)
        {
            Building? building = _dbContext.Buildings.FirstOrDefault(b => b.Id == id);
            return building == null ? null : _mapper.Map<BuildingDto>(building);
        }

        public async Task<bool> UpdateBuildingAsync(BuildingDto buildingDto)
        {
            Building? existingBuilding = await _dbContext.Buildings.FirstOrDefaultAsync(b=> b.Id == buildingDto.Id);

            if(existingBuilding == null)
            {
                return false;
            }
            existingBuilding.Name = buildingDto.Name;
            existingBuilding.Floors = buildingDto.Floors;
            existingBuilding.YearBuilt = buildingDto.YearBuilt;
           _dbContext.Buildings.Update(existingBuilding);
            int countUpdated =  await _dbContext.SaveChangesAsync();
            return countUpdated>0;
        }

        List<BuildingDto> IBuildingsRepository.GetFilteredAndSortedBuildings(BuildingsRequestData requestData)
        {
            IQueryable<Building> query = _dbContext.Buildings;

            // Filter

            if (requestData.Filters.Names.Any())
            {
                query = query.Where(b => requestData.Filters.Names.Contains(b.Name));
            }

                if (requestData.Filters.Addresses != null && requestData.Filters.Addresses.Any())
            {
                query = query.Where(b => requestData.Filters.Addresses.Contains(b.Street));
            }

            if (requestData.Filters.MinFloors.HasValue)
            {
                query = query.Where(b => b.Floors >= requestData.Filters.MinFloors.Value);
            }

            if (requestData.Filters.MaxFloors.HasValue)
            {
                query = query.Where(b => b.Floors <= requestData.Filters.MaxFloors.Value);
            }

            if (requestData.Filters.MinBuiltYear.HasValue)
            {
                query = query.Where(b => b.YearBuilt >= requestData.Filters.MinBuiltYear.Value);
            }

            if (requestData.Filters.MaxBuiltYear.HasValue)
            {
                query = query.Where(b => b.YearBuilt <= requestData.Filters.MaxBuiltYear.Value);
            }

            // Sort
            if (requestData.OrderBy != BuildingOrderBy.None && requestData.SortOrder != SortOrder.None)
            {
                query = requestData.OrderBy switch
                {
                    BuildingOrderBy.Name => requestData.SortOrder == SortOrder.Ascending
                        ? query.OrderBy(b => b.Name)
                        : query.OrderByDescending(b => b.Name),

                    BuildingOrderBy.Address => requestData.SortOrder == SortOrder.Ascending
                        ? query.OrderBy(b => b.City).ThenBy(b => b.Street).ThenBy(b => b.BuildingNumber)
                        : query.OrderByDescending(b => b.City).ThenByDescending(b => b.Street).ThenByDescending(b => b.BuildingNumber),

                    BuildingOrderBy.Floors => requestData.SortOrder == SortOrder.Ascending
                        ? query.OrderBy(b => b.Floors)
                        : query.OrderByDescending(b => b.Floors),

                    BuildingOrderBy.YearBuilt => requestData.SortOrder == SortOrder.Ascending
                        ? query.OrderBy(b => b.YearBuilt)
                        : query.OrderByDescending(b => b.YearBuilt),

                    _ => query
                };
            }
            List<Building> buildingsList = query.ToList();
            return _mapper.Map<List<BuildingDto>>(buildingsList);
            
        }
    }
}
