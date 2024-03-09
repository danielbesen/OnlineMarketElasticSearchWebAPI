using DanielMarket.Models;
using DanielMarket.Models.Utils;
using DanielMarket.Services;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Retrive all documents")]
        public async Task<ActionResult> GetAllDocuments()
        {
            try
            {
                var documents = await _elasticSearchService.GetAllDocumentsAsync("products");
                ResponseResult<Product> response = new ResponseResult<Product>(documents);
                if (response.Results == null || response.TotalCount == 0)
                    return NotFound();
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }

        [HttpGet]
        [Route("GetDocumentsByField/{fieldName}/{fieldValue}")]
        [SwaggerOperation(Summary = "Retrive documents by exact match field")]
        public async Task<ActionResult> GetDocumentsByField(string fieldName, string fieldValue)
        {
            try
            {
                var documents = await _elasticSearchService.GetDocumentsByTermAsync("products", fieldName, fieldValue);
                ResponseResult<Product> response = new ResponseResult<Product>(documents);
                if (response.Results == null || response.TotalCount == 0)
                    return NotFound();
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }

        [HttpPost]
        [Route("GetDocumentsByTerms/{fieldName}")]
        [SwaggerOperation(Summary = "Retrive documents by exact match fields (operator OR)")]
        public async Task<ActionResult> GetDocumentsByTerms(string fieldName, [FromBody] List<string> fieldValue)
        {
            try
            {
                var documents = await _elasticSearchService.GetDocumentsByTermsAsync("products", fieldName, fieldValue);
                ResponseResult<Product> response = new ResponseResult<Product>(documents);
                if (response.Results == null || response.TotalCount == 0)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }

        [HttpPost]
        [Route("GetDocumentsByIds")]
        [SwaggerOperation(Summary = "Retrive documents by their IDs")]
        public async Task<ActionResult> GetDocumentsByIds( [FromBody] List<string> fieldValue)
        {
            try
            {
                var documents = await _elasticSearchService.GetDocumentsByIdsAsync("products", fieldValue);
                ResponseResult<Product> response = new ResponseResult<Product>(documents);
                if (response.Results == null || response.TotalCount == 0)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }

        [HttpGet]
        [Route("GetDocumentsGreaterThan/{fieldName}/{fieldValue}")]
        [SwaggerOperation(Summary = "Retrive documents with fieldName greater than fieldValue")]
        public async Task<ActionResult> GetDocumentsGreaterThan(string fieldName, string fieldValue)
        {
            try
            {
                var documents = await _elasticSearchService.GetDocumentsGreaterThan("products", fieldName, fieldValue);
                ResponseResult<Product> response = new ResponseResult<Product>(documents);
                if (response.Results == null || response.TotalCount == 0)
                    return NotFound();
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }

        [HttpGet]
        [Route("GetDocumentsByPrefix/{fieldName}/{fieldValue}")]
        [SwaggerOperation(Summary = "Retrive documents by prefix")]
        public async Task<IActionResult> GetDocumentsByPrefix(string fieldName, string fieldValue)
        {
            try
            {
                var documents = await _elasticSearchService.GetDocumentsByPrefix("products", fieldName, fieldValue);
                ResponseResult<Product> response = new ResponseResult<Product>(documents);
                if (response == null || response.TotalCount == 0)
                    return NotFound(response);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }

        [HttpGet]
        [Route("GetDocumentsWithNotNullField/{fieldName}")]
        [SwaggerOperation(Summary = "Retrive documents with not null field")]
        public async Task<IActionResult> GetDocumentsWithNotNullField(string fieldName)
        {
            try
            {
                var documents = await _elasticSearchService.GetDocumentsWithNotNullField("products", fieldName);
                ResponseResult<Product> response = new ResponseResult<Product>(documents);
                if (response == null || response.TotalCount == 0)
                    return NotFound(response);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }

        [HttpGet]
        [Route("GetDocumentsWithNullField/{fieldName}")]
        [SwaggerOperation(Summary = "Retrive documents with null field")]
        public async Task<IActionResult> GetDocumentsWithNullField(string fieldName)
        {
            try
            {
                var documents = await _elasticSearchService.GetDocumentsWithNullField("products", fieldName);
                ResponseResult<Product> response = new ResponseResult<Product>(documents);
                if (response == null || response.TotalCount == 0)
                    return NotFound(response);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }
    }
}
