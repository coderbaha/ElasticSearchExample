using ElasticSearchExample.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticSearchExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class ProductController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IElasticClient elasticClient, ILogger<ProductController> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }
        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts(string keyword)
        {
            var results = await _elasticClient.SearchAsync<Product>(
                s => s.Query(
                    q => q.QueryString(
                        d => d.Query('*' + keyword + '*'))
                    ).Size(1000)
                );
            return Ok(results.Documents.ToList());
        }
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            await _elasticClient.IndexDocumentAsync(product);
            return Ok();
        }
    }
}
