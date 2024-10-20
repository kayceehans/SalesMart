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

        public SalesOrderService(IGenericRepo<SalesOrder> SalesOrderrepo)
        {
            _SalesOrderrepo = SalesOrderrepo;
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
                var response = new Result<SalesOrder>();
                response.IsSuccess = false;
                response.ErrorMessage = $"Error occured:{ex.Message}";
                return response;
            }
        }
    }
}
