using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Domain.Entities
{
    public class SalesOrder
    {
        public int Id { get; set; }
        public long Quantity { get; set; }
        public string ProductId { get; set; }
        public string CustomerId { get; set; }
    }
}
