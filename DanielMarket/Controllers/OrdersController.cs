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
                var stats = await _elasticSearchService.GetStatsExplicity("orders", fieldName);
                if (stats == null)
                    return NotFound(stats);
                return Ok(stats);
            }
            catch (Exception e)
            {
                throw new Exception($"Error : {e}");
            }
        }
    }
}
