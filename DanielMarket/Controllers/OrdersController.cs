using DanielMarket.Models;
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
    }
}
