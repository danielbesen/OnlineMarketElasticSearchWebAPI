using DanielMarket.Models;
using DanielMarket.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace DanielMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private string indexName = "department";
        private readonly IElasticSearchService<Department> _elasticSearchService;

        public DepartmentsController(IElasticSearchService<Department> elasticSearchService)
        {
            this._elasticSearchService = elasticSearchService;
        }

        [HttpGet]
        [Route("GetNestedAggregation/{pathName}/{fieldName}")]
        public async Task<ActionResult> GetNestedAggregation(string pathName, string fieldName)
        {
            try
            {
                var aggsValues = await _elasticSearchService.GetNestedAggregationAsync(indexName, pathName, fieldName);
                if (aggsValues == null || aggsValues.Count == 0)
                    return NotFound(aggsValues);
                return Ok(aggsValues);
            }
            catch (Exception e)
            {
                throw new Exception($"Error : {e}");
            }
        }
    }
}
