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
    public interface ISalesOrderService
    {
        Task<Result<List<SalesOrder>>> GetAllSales();
        Task<Result<string>> AddSales(SalesOrderDto sales);
        Task<Result<SalesOrder>> GetSalesById(int id);       
    }
}
