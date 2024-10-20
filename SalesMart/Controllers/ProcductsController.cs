using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesMart.Domain.DataTransferObject;
using SalesMart.Service.Interface;

namespace SalesMart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProcductsController(IProductService productService)
        {
            _productService = productService;    
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddPorduct(ProductDto request)
        {
            try
            {

                return Ok(await _productService.CreateProduct(request));    
            }
            catch (Exception ex)
            {
               return BadRequest(ex.InnerException);
            }
        }

        [HttpGet]
        [Route("GetProducts")]
        public async Task<IActionResult> GetPorduct()
        {
            try
            {

                return Ok(await _productService.GetProducts());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }
        [HttpGet]
        [Route("GetProductsById")]
        public async Task<IActionResult> GetPorductById(int id)
        {
            try
            {

                return Ok(await _productService.GetProductById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }
    }
}
