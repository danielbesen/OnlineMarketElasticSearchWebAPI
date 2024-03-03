using DanielMarket.Models;
using DanielMarket.Models.Utils;
using DanielMarket.Services;
using Microsoft.AspNetCore.Mvc;
using Nest;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DanielMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly Services.IElasticSearchService<Product> _elasticSearchService;

        public ProductsController(IElasticSearchService<Product> elasticSearchService)
        {
            this._elasticSearchService = elasticSearchService;
        }

        [HttpGet]
        [Route("GetAllDocuments")]
        public async Task<ActionResult> GetAllDocuments()
        {
            var documents = await _elasticSearchService.GetAllDocumentsAsync("products");
            ResponseResult<Product> response = new ResponseResult<Product>(documents);
            if (response.Results == null || response.TotalCount == 0)
                return NotFound();
            return Ok(response);
        }

        [HttpGet]
        [Route("GetDocumentsByField/{fieldName}/{fieldValue}")]
        public async Task<ActionResult> GetDocumentsByField(string fieldName, string fieldValue)
        {
            var documents = await _elasticSearchService.GetDocumentsByTermAsync("products", fieldName, fieldValue);
            ResponseResult<Product> response = new ResponseResult<Product>(documents);
            if (response.Results == null || response.TotalCount == 0)
                return NotFound();
            return Ok(response);
        }

        [HttpPost]
        [Route("GetDocumentsByTerms/{fieldName}")]
        public async Task<ActionResult> GetDocumentsByTerms(string fieldName, [FromBody] List<string> fieldValue)
        {
            var documents = await _elasticSearchService.GetDocumentsByTermsAsync("products", fieldName, fieldValue);
            ResponseResult<Product> response = new ResponseResult<Product>(documents);
            if (response.Results == null || response.TotalCount == 0) { 
                return NotFound();}
            return Ok(response);           
        }
    }
}
