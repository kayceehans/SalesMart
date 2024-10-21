using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SalesMart.Domain.Common.Generic;
using SalesMart.Domain.DataTransferObject;
using SalesMart.Domain.Entities;
using SalesMart.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Service.Implementation
{
    public class DashBoardService : IDashBoardService
    {
        private readonly ISalesOrderService _salesOrderService;
        private readonly IProductService _productService;
        private ILogger<DashBoardService> _logger;
        public DashBoardService(ISalesOrderService salesOrderService, IProductService productService, ILogger<DashBoardService> logger)
        {
            _salesOrderService = salesOrderService;
            _productService = productService;
            _logger = logger;
        }

        public async Task<Result<DashboardItemDto>> GetDashboardItem()
        {
            try
            {
                //Retrieve all Products and Sales
                var getSalesOrders = await _salesOrderService.GetAllSales();
                var getAllProducts = await _productService.GetProducts();

                // Get the product ID of the Sales with the Highest Quantity Sold
                var productId = (getSalesOrders.Content == null) ? 0 : getSalesOrders.Content.OrderByDescending(c => c.Quantity).FirstOrDefault().ProductId;

                //Get the product with the highest Quantity with the product ID

                var mostSold = (productId != 0 && getAllProducts.Content != null) ? getAllProducts.Content.Where(c => c.Id == productId).FirstOrDefault() : null;

                //Get product with the Highest Price

                var mostExpensive = (getAllProducts.Content != null) ? getAllProducts.Content.OrderByDescending(c => c.Price).FirstOrDefault() : null;


                return new Result<DashboardItemDto>
                {
                    Content = new DashboardItemDto
                    {
                        MostExpensive = $"Product with Highest Price:{JsonConvert.SerializeObject(mostExpensive)}",
                        MostSoldProduct = $"Product with Highest Quantity Sold:{JsonConvert.SerializeObject(mostSold)}"
                    },
                    Message = "Dash items",
                    ErrorMessage = (mostExpensive == null && mostSold == null) ? "Unable to get Dash Board items" : (mostSold == null) ? "Unable to get Product with Highest Quantity" : (mostExpensive == null) ? "Unable to get Product with Highest Price" : string.Empty,
                    IsSuccess = (mostExpensive != null && mostSold != null)
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<DashboardItemDto>() { ErrorMessage = ex.Message };
            }
        }
    }
}
