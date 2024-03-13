using DanielMarket.Models;
using DanielMarket.Models.Utils;
using DanielMarket.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DanielMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IElasticSearchService<Order> _elasticSearchService;

        public OrdersController(IElasticSearchService<Order> elasticSearchService)
        {
            this._elasticSearchService = elasticSearchService;
        }

        [HttpGet]
        [Route("GetStatsExplicity/{fieldName}")]
        public async Task<ActionResult> GetStatsExplicity(string fieldName)
        {
            try
            {
                var aggsValues = await _elasticSearchService.GetStatsExplicityAsync("orders", fieldName);
                if (aggsValues == null)
                    return NotFound(aggsValues);
                return Ok(aggsValues);
            }
            catch (Exception e)
            {
                throw new Exception($"Error : {e}");
            }
        }

        [HttpGet]
        [Route("GetHowManyDifferentValues/{fieldName}")]
        public async Task<ActionResult> GetHowManyDifferentValues(string fieldName)
        {
            try
            {
                var aggsValues = await _elasticSearchService.GetHowManyDifferentValuesAsync("orders", fieldName);
                if (aggsValues == null)
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
