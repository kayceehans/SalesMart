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
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderedDate { get; set; }
    }
}
