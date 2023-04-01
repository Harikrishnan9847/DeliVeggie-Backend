using System.Collections.Generic;
using System.Threading.Tasks;
using DeliVeggie.API.Filter;
using DeliVeggie.Domain.Contracts.Product;
using DeliVeggie.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace DeliVeggie.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        #region PRIVATE PROPERTIES
        private readonly IProductService _productService;
        #endregion

        #region CONSTRUCTOR
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region PUBLIC METHODS
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProducts();
            if (!(products is Response<List<ProductResponse>> response))
            {
                return NotFound();
            }
            return Ok(response.Data);
        }
        [HttpGet("Get/{id}")]
        [ProductIdFilter]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _productService.GetProduct(id);
            if (!(product is Response<ProductDetailsResponse> response))
            {
                return NotFound();
            }
            return Ok(response.Data);
        }
        #endregion
    }
}
