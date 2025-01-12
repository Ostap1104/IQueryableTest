using Microsoft.AspNetCore.Mvc;
using QueryableCore.DTOs;
using QueryableCore.Services;
using QueryableCore.Services.Interfaces;
using Shared;

namespace QueryableAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuildingsController : ControllerBase
    {
        private readonly IBuildingsService _buildingsService;

        public BuildingsController(IBuildingsService buildingsService)
        {
            _buildingsService = buildingsService;
        }

        [HttpPost]
        [Route("get")]
        public IActionResult Get([FromBody] BuildingsRequestData requestData)
        {
            var buildings = _buildingsService.GetBuildings(requestData);
            return Ok(buildings);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult CreateBuilding([FromBody] BuildingDto buildingDto)
        {
            int? id = _buildingsService.CreateBuilding(buildingDto);

            return id.HasValue ? Ok(id) : BadRequest();
        }

        [HttpGet("get-list-of-required-fields-for-buildingdto")]
        public IActionResult GetListOfRequiredFieldsForBuildingDto([FromQuery] string modelName)
        {
            if (string.IsNullOrWhiteSpace(modelName))
                return BadRequest("Model name cannot be null or empty.");

            var modelType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name.Equals(modelName, StringComparison.OrdinalIgnoreCase));

            if (modelType == null)
                return NotFound($"Model type '{modelName}' not found.");

            var requiredFields = _buildingsService.GetRequiredFields(modelType);

            return Ok(requiredFields);
        }

        [HttpGet("get-list-of-class-members")]
        public IActionResult GetListOfClassMembers(
        [FromQuery] string modelName,
        [FromQuery] bool? isRequired = null,
        [FromQuery] AccessModifier[] accessModifiers = null,
        [FromQuery] MemberType[] memberTypes = null,
        [FromQuery] bool? isStatic = null)
        {
            if (string.IsNullOrWhiteSpace(modelName))
                return BadRequest("Model name cannot be null or empty.");

            try
            {
                var result = _buildingsService.GetClassMembers(modelName, isRequired, accessModifiers, memberTypes, isStatic);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
