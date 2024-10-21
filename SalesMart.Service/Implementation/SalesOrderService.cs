using Microsoft.Extensions.Logging;
using SalesMart.Data.Repositories;
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
    public class SalesOrderService : ISalesOrderService
    {
        private readonly IGenericRepo<SalesOrder> _SalesOrderrepo;
        private readonly ILogger<SalesOrderService> _logger;
        public SalesOrderService(IGenericRepo<SalesOrder> SalesOrderrepo, ILogger<SalesOrderService> logger)
        {
            _SalesOrderrepo = SalesOrderrepo;
            _logger = logger;
        }
        public async Task<Result<List<SalesOrder>>> GetAllSales()
        {
            try
            {
                var response = new Result<List<SalesOrder>>();
                var sales = await _SalesOrderrepo.GetAllAsync();
                if (sales != null)
                {
                    response.Content = sales;
                    response.IsSuccess = true;
                    response.Message = $"List of all available Sales. Total:{sales.Count}";
                }
                else
                {
                    response.Content = null;
                    response.IsSuccess = false;
                    response.Message = "Unable to get Sales records";
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                var response = new Result<List<SalesOrder>>();
                response.IsSuccess = false;
                response.ErrorMessage = $"Error occured:{ex.Message}";
                return response;
            }
        }
        public async Task<Result<string>> AddSales(SalesOrderDto sales)
        {
            try
            {
                var response = new Result<string>();
                var request = new SalesOrder
                {
                    CustomerId = sales.CustomerId,
                    ProductId = sales.ProductId,
                    Quantity = sales.Quantity,
                    OrderedDate = DateTime.Now
                };

                _SalesOrderrepo.Add(request);
                var isAdded = await _SalesOrderrepo.SaveAsync();

                if (isAdded > 0)
                {
                    response.Content = "Sales added successfully";
                    response.IsSuccess = true;
                    response.Message = $"Success";
                }
                else
                {
                    response.Content = "Adding Sales fail.";
                    response.IsSuccess = false;
                    response.Message = "Please try again";
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                var response = new Result<string>();
                response.IsSuccess = false;
                response.ErrorMessage = $"Error occured:{ex.Message}";
                return response;
            }
        }
        public async Task<Result<SalesOrder>> GetSalesById(int id)
        {
            try
            {
                var response = new Result<SalesOrder>();
                var getSales = await _SalesOrderrepo.SelectAsync(c => c.Id == id);
                if (getSales != null)
                {
                    response.Content = getSales;
                    response.IsSuccess = true;
                    response.Message = "Sale retrieved successfully";
                }
                else
                {
                    response.Content = null;
                    response.IsSuccess = false;
                    response.Message = "Unable to get sales record";
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                var response = new Result<SalesOrder>();
                response.IsSuccess = false;
                response.ErrorMessage = $"Error occured:{ex.Message}";
                return response;
            }
        }
        public async Task<Result<string>> DeleteSalesById(int id)
        {
            try
            {
                var response = new Result<string>();

                var getSales = await _SalesOrderrepo.SelectAsync(x => x.Id == id);
                if (getSales == null)
                {
                    response.Content = $"Sales with Id:{id} not found";
                    response.IsSuccess = true;
                    response.Message = "Sale not found";                    
                    return response;
                }

                _SalesOrderrepo.Remove(getSales.Id);
                var Update = await _SalesOrderrepo.SaveAsync() > 0;               

                if (Update)
                {
                    response.Content = $"Sales with Id:{id} removed";
                    response.IsSuccess = true;
                    response.Message = "Sale deleted successfully";
                }
                else
                {
                    response.Content = $"Delete failed for sale with Id:{id}"; ;
                    response.IsSuccess = false;
                    response.Message = "Unable to remove sales record, please try again";
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                var response = new Result<string>();
                response.IsSuccess = false;
                response.ErrorMessage = $"Error occured:{ex.Message}";
                return response;
            }
        }
    }
}
