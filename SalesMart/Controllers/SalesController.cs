using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesMart.Domain.DataTransferObject;
using SalesMart.Service.Implementation;
using SalesMart.Service.Interface;

namespace SalesMart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISalesOrderService _salesOrderService;

        public SalesController(ISalesOrderService salesOrderService)
        {
            _salesOrderService = salesOrderService;  
        }

        [HttpPost]
        [Route("AddSales")]
        public async Task<IActionResult> AddSalesOrder(SalesOrderDto request)
        {
            try
            {

                return Ok(await _salesOrderService.AddSales(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }

        [HttpGet]
        [Route("GetSales")]
        public async Task<IActionResult> GetSales()
        {
            try
            {

                return Ok(await _salesOrderService.GetAllSales());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }
        [HttpGet]
        [Route("GetSalesById")]
        public async Task<IActionResult> GetSalesById(int id)
        {
            try
            {

                return Ok(await _salesOrderService.GetSalesById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }
    }
}
