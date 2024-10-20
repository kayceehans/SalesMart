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
    public class ProductService : IProductService
    {
        private readonly IGenericRepo<Product> _productRepo;
        public ProductService(IGenericRepo<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<Result<List<Product>>> GetProducts()
        {
            try
            {
                var response = new Result<List<Product>>();
                var products = await _productRepo.GetAllAsync();
                if (products != null)
                {
                    response.Content = products;
                    response.IsSuccess = true;
                    response.Message = $"List of all available products. Total:{products.Count}";
                }
                else
                {
                    response.Content = null;
                    response.IsSuccess = false;
                    response.Message = "Unable to get products";
                }
                return response;
            }
            catch (Exception ex)
            {
                var response = new Result<List<Product>>();
                response.IsSuccess = false;
                response.ErrorMessage = $"Error occured:{ex.Message}";
                return response;
            }
        }
        public async Task<Result<string>> CreateProduct(ProductDto request)
        {
            try
            {
                var response = new Result<string>();
                var addProduct = new Product
                {
                    Name = request.Name,
                    Price = request.Price
                };
                _productRepo.Add(addProduct);
                int saved = await _productRepo.SaveAsync();

                if (saved > 0)
                {
                    response.Content = $"Product:{request.Name} added";
                    response.IsSuccess = true;
                    response.Message = $"product added succssfully";
                }
                else
                {
                    response.Content = null;
                    response.IsSuccess = false;
                    response.Message = $"Unable to add product{request.Name}";
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
        public async Task<Result<Product>> GetProductById(int productId)
        {
            try
            {
                var response = new Result<Product>();
                var products = await _productRepo.SelectAsync(c => c.Id == productId);
                if (products != null)
                {
                    response.Content = products;
                    response.IsSuccess = true;
                    response.Message = $"product retrieved successfully.";
                }
                else
                {
                    response.Content = null;
                    response.IsSuccess = false;
                    response.Message = "Unable to get products";
                }
                return response;
            }
            catch (Exception ex)
            {
                var response = new Result<Product>();
                response.IsSuccess = false;
                response.ErrorMessage = $"Error occured:{ex.Message}";
                return response;
            }
        }
        public async Task<Result<string>> RemoveProduct(int productId)
        {
            try
            {
                var response = new Result<string>();
                _productRepo.Remove(productId);
                var isRemoved = await _productRepo.SaveAsync();
                if (isRemoved > 0)
                {
                    response.Content = $"Item id:{productId} removed.";
                    response.IsSuccess = true;
                    response.Message = $"Item removed successfully.";
                }
                else
                {
                    response.Content = null;
                    response.IsSuccess = false;
                    response.Message = $"Unable to remove product with id: {productId}";
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
    }
}
