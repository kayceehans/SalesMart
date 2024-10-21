﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Domain.DataTransferObject
{
    public class SalesOrderDto
    {
        public long Quantity { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
    }
}
