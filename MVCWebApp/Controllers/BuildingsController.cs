using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVCWebApp.Models;
using QueryableCore.DTOs;
using QueryableCore.Services.Interfaces;
using Shared;

namespace MVCWebApp.Controllers
{
    public class BuildingsController : Controller
    {
        private readonly IBuildingsService _buildingsService;

        private readonly IMapper _mapper;

        public BuildingsController(IBuildingsService buildingsService, IMapper mapper)
        {
            _buildingsService = buildingsService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {

            List<BuildingDto> buildings = _buildingsService.GetBuildings(new BuildingsRequestData());

            List<BuildingModel> buildingModels = _mapper.Map<List<BuildingModel>>(buildings);

            return View(buildingModels);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            BuildingDto? building = _buildingsService.Get(id);
            if (building == null)
            {
                return NotFound();
            }
            BuildingModel buildingModel = _mapper.Map<BuildingModel>(building);
            return View(buildingModel);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            BuildingDto? building = _buildingsService.Get(id);
            if (building == null)
            {
                return NotFound();
            }
            BuildingModel buildingModel = _mapper.Map<BuildingModel>(building);
            return View(buildingModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(BuildingModel buildingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(buildingModel);
            }
            BuildingDto buildingDto = _mapper.Map<BuildingDto>(buildingModel);


            bool isSuccess = await _buildingsService.UpdateBuildingAsync(buildingDto);

            //if (isSuccess)
            //{
            //    return RedirectToAction("Index");
            //}
            //else
            //{
               
            //}
            return isSuccess ? RedirectToAction("Index") : View(buildingModel);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            bool isSuccess = await _buildingsService.DeleteAsync(id);
            if (isSuccess)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
