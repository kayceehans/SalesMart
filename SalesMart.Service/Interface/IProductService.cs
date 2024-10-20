using SalesMart.Domain.Common.Generic;
using SalesMart.Domain.DataTransferObject;
using SalesMart.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Service.Interface
{
    public interface IProductService
    {
        Task<Result<List<Product>>> GetProducts();
        Task<Result<string>> CreateProduct(ProductDto request);
        Task<Result<Product>> GetProductById(int productId);        
        Task<Result<string>> RemoveProduct(int productId);
    }
}
